using FluentResults;
using MediatR;
using Pilotiv.AuthorizationAPI.Application.Requests.Authorization.Commands.Authorize.Dtos;
using Pilotiv.AuthorizationAPI.Application.Shared.Helpers;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Commands;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Queries;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Jwt.Abstractions;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Authorization.Commands.Authorize;

/// <summary>
/// Обработчик команды авторизации.
/// </summary>
public class AuthorizeCommandHandler : IRequestHandler<AuthorizeCommand, Result<AuthorizeCommandResponse>>
{
    private readonly IUsersCommandsRepository _usersCommandsRepository;
    private readonly IUsersQueriesRepository _usersQueriesRepository;
    private readonly IJwtProvider _jwtProvider;

    /// <summary>
    /// Создание обработчика команды авторизации.
    /// </summary>
    /// <param name="jwtProvider">Сервис работы с токенами.</param>
    /// <param name="usersCommandsRepository">Интерфейс репозитория команд пользователей.</param>
    /// <param name="usersQueriesRepository">Интерфейс репозитория запросов пользователей.</param>
    public AuthorizeCommandHandler(IJwtProvider jwtProvider, IUsersCommandsRepository usersCommandsRepository,
        IUsersQueriesRepository usersQueriesRepository)
    {
        _jwtProvider = jwtProvider;
        _usersCommandsRepository = usersCommandsRepository;
        _usersQueriesRepository = usersQueriesRepository;
    }

    /// <summary>
    /// Обработка команды авторизации.
    /// </summary>
    /// <param name="request">Команда авторизации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объекта переноса данных ответа команды авторизации.</returns>
    public async Task<Result<AuthorizeCommandResponse>> Handle(AuthorizeCommand request,
        CancellationToken cancellationToken = default)
    {
        var getUserLoginResult = UserLogin.Create(request.Login);
        if (getUserLoginResult.IsFailed)
        {
            return Result.Fail(getUserLoginResult.Errors);
        }
        
        var userLogin = getUserLoginResult.ValueOrDefault;

        var getUserResult = await _usersQueriesRepository.GetUserByLoginPasswordAsync(userLogin, request.Password);
        if (getUserResult.IsFailed)
        {
            return getUserResult.ToResult();
        }

        var user = getUserResult.ValueOrDefault;

        var accessToken = TokensHelper.GenerateAccessTokenForUser(_jwtProvider, user);
        var createRefreshTokenResult = TokensHelper.CreateRefreshToken(_jwtProvider, request.Ip);
        if (createRefreshTokenResult.IsFailed)
        {
            return createRefreshTokenResult.ToResult();
        }

        var refreshToken = createRefreshTokenResult.ValueOrDefault;
        var addRefreshTokenResult = user.AddRefreshToken(refreshToken);
        if (addRefreshTokenResult.IsFailed)
        {
            return addRefreshTokenResult;
        }

        await _usersCommandsRepository.CommitChangesAsync(user, cancellationToken);

        return new AuthorizeCommandResponse
        {
            AccessToken = accessToken,
            RefreshToken = new()
            {
                Token = refreshToken.Id.Value,
                Expires = refreshToken.ExpirationDate
            }
        };
    }
}