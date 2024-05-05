using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.RevokeRefreshToken.Errors;

public static class RevokeRefreshTokenErrors
{
    public static Error RevokingTokenNotFound(User user, RefreshTokenId refreshTokenId) =>
        new($"Не удалось обнаружить отзываемый токен доступа ({refreshTokenId.Value}) для пользователя ({user.Id})");
}