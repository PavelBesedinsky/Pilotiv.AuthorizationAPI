using FluentResults;
using MediatR;
using Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.RefreshTokens.Dtos;
using Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.RefreshTokens.Errors;
using Pilotiv.AuthorizationAPI.Application.Shared.Helpers;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Commands;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Queries;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Entities;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Jwt.Abstractions;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.RefreshTokens;

/// <summary>
/// Обработчик команды обновления токенов.
/// </summary>
public class RefreshTokensCommandHandler : IRequestHandler<RefreshTokensCommand, Result<RefreshTokensCommandResponse>>
{
    private readonly IJwtProvider _jwtProvider;
    private readonly IUsersQueriesRepository _usersQueriesRepository;
    private readonly IUsersCommandsRepository _usersCommandsRepository;

    /// <summary>
    /// Создание обработчика команды обновления токенов.
    /// </summary>
    /// <param name="jwtProvider">Сервис работы с токенами.</param>
    /// <param name="usersQueriesRepository">Репозиторий запросов пользователя.</param>
    /// <param name="usersCommandsRepository">Репозиторий команд пользователя.</param>
    public RefreshTokensCommandHandler(IJwtProvider jwtProvider, IUsersQueriesRepository usersQueriesRepository,
        IUsersCommandsRepository usersCommandsRepository)
    {
        _jwtProvider = jwtProvider;
        _usersQueriesRepository = usersQueriesRepository;
        _usersCommandsRepository = usersCommandsRepository;
    }

    /// <summary>
    /// Обработка команды обновления токенов.
    /// </summary>
    /// <param name="request">Команда обновления токенов.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объекта переноса данных ответа команды обновления токенов.</returns>
    public async Task<Result<RefreshTokensCommandResponse>> Handle(RefreshTokensCommand request,
        CancellationToken cancellationToken)
    {
        var refreshTokenId = RefreshTokenId.Create(request.RefreshToken);
        var getUserResult = await _usersQueriesRepository.GetUserByRefreshTokenAsync(refreshTokenId);
        if (getUserResult.IsFailed)
        {
            return getUserResult.ToResult();
        }

        var user = getUserResult.ValueOrDefault;

        var revokingToken = user.RefreshTokens.SingleOrDefault(refreshToken => refreshToken.Id == refreshTokenId);
        if (revokingToken is null)
        {
            return RefreshTokensErrors.RevokingTokenNotFound(user, refreshTokenId);
        }

        var accessToken = TokensHelper.GenerateAccessTokenForUser(_jwtProvider, user);
        var createRefreshTokenResult = TokensHelper.CreateRefreshToken(_jwtProvider, request.Ip);
        if (createRefreshTokenResult.IsFailed)
        {
            return createRefreshTokenResult.ToResult();
        }

        var replacingToken = createRefreshTokenResult.ValueOrDefault;

        var ip = request.Ip ?? "0.0.0.0";

        var replaceTokenResult = ReplaceRefreshToken(user, revokingToken, replacingToken, ip);
        if (replaceTokenResult.IsFailed)
        {
            return replaceTokenResult;
        }

        await _usersCommandsRepository.CommitChangesAsync(user, cancellationToken);

        return new RefreshTokensCommandResponse
        {
            AccessToken = accessToken,
            RefreshToken = new()
            {
                Token = replacingToken.Id.Value,
                Expires = replacingToken.ExpirationDate
            }
        };
    }

    /// <summary>
    /// Выполнение замены токена обновления.
    /// </summary>
    /// <param name="user">Пользователь, токен которого надо заменить.</param>
    /// <param name="revokingToken">Отзываемый токен.</param>
    /// <param name="replacingToken">Заменяющий токен.</param>
    /// <param name="ip">IP-адрес, с которого выполняется отзыв токена.</param>
    private static Result ReplaceRefreshToken(User user, RefreshToken revokingToken, RefreshToken replacingToken,
        string ip)
    {
        if (!revokingToken.IsRevoked)
        {
            // Если токен уже отозван, то нужно отозвать все дочерние токены, так как токен может быть скомпроментирован.
            var revokeDescendantTokensResult = RevokeDescendantTokens(user, revokingToken, ip,
                "Попытка обновления токена с использованием отозванного токена");
            if (revokeDescendantTokensResult.IsFailed)
            {
                return revokeDescendantTokensResult;
            }
        }

        var revokeTokenResult = user.RevokeToken(revokingToken, ip, "Замена токена", replacingToken);
        if (revokeTokenResult.IsFailed)
        {
            return revokeTokenResult;
        }

        return Result.Ok();
    }

    /// <summary>
    /// Отзыв дочерних токенов.
    /// </summary>
    /// <param name="user">Пользователь, токен которого надо отозвать.</param>
    /// <param name="revokingToken">Отзываемый токен.</param>
    /// <param name="ip">IP-адрес, с которого выполняется отзыв токена.</param>
    /// <param name="revokeReason">Причина отзыва токена.</param>
    private static Result RevokeDescendantTokens(User user, RefreshToken revokingToken, string ip, string revokeReason)
    {
        while (true)
        {
            var childToken = revokingToken.ReplacingToken;
            if (childToken is null)
            {
                return Result.Ok();
            }

            if (!childToken.IsActive)
            {
                continue;
            }

            var revokeTokenResult = user.RevokeToken(childToken, ip, revokeReason);
            if (revokeTokenResult.IsFailed)
            {
                return revokeTokenResult;
            }
        }
    }
}