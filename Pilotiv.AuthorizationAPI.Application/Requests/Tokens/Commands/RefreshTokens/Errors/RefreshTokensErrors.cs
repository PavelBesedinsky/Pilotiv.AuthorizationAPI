using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.RefreshTokens.Errors;

public static class RefreshTokensErrors
{
    public static Error RevokingTokenNotFound(User user, RefreshTokenId refreshTokenId) => new(
        $"Не удалось найти отзываемый токен обновления ({refreshTokenId.Value}) для пользователя ({user.Id})");
}