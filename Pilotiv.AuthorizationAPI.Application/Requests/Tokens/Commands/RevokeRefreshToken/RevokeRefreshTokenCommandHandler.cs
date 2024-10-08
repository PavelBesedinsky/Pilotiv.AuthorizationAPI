﻿using FluentResults;
using MediatR;
using Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.RevokeRefreshToken.Errors;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Commands;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Queries;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.RevokeRefreshToken;

/// <summary>
/// Обработчик команды отзыва токена обновления.
/// </summary>
public class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand, Result>
{
    private readonly IUsersCommandsRepository _usersCommandsRepository;
    private readonly IUsersQueriesRepository _usersQueriesRepository;

    /// <summary>
    /// Создание обработчика команды отзыва токена обновления.
    /// </summary>
    /// <param name="usersCommandsRepository">Репозиторий команд пользователя.</param>
    /// <param name="usersQueriesRepository">Репозиторий запросов пользователя.</param>
    public RevokeRefreshTokenCommandHandler(IUsersCommandsRepository usersCommandsRepository,
        IUsersQueriesRepository usersQueriesRepository)
    {
        _usersCommandsRepository = usersCommandsRepository;
        _usersQueriesRepository = usersQueriesRepository;
    }

    /// <summary>
    /// Обработка команды отзыва токена обновления.
    /// </summary>
    /// <param name="request">Команда отзыва токена обновления.</param>
    /// <param name="cancellationToken">Токен обновления.</param>
    public async Task<Result> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken = default)
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
            return RevokeRefreshTokenErrors.RevokingTokenNotFound(user, refreshTokenId);
        }

        var ip = request.Ip ?? "0.0.0.0";
        var reason = request.Reason ?? "Причина не указана";

        var revokeTokenResult = user.RevokeToken(revokingToken, ip, reason);
        if (revokeTokenResult.IsFailed)
        {
            return revokeTokenResult;
        }

        await _usersCommandsRepository.CommitChangesAsync(user, cancellationToken);

        return Result.Ok();
    }
}