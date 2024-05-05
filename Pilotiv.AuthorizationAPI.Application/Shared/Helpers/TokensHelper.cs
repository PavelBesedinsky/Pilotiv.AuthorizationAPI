using FluentResults;
using Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Entities;
using Pilotiv.AuthorizationAPI.Jwt.Abstractions;

namespace Pilotiv.AuthorizationAPI.Application.Shared.Helpers;

/// <summary>
/// Вспомогательный класс работы с токенами.
/// </summary>
internal static class TokensHelper
{
    /// <summary>
    /// Создание токена доступа для пользователя.
    /// </summary>
    /// <param name="jwtProvider">Сервис работы с токенами.</param>
    /// <param name="user">Пользователь.</param>
    /// <returns>Токен доступа.</returns>
    internal static string GenerateAccessTokenForUser(IJwtProvider jwtProvider, User user)
    {
        return jwtProvider.GenerateAccessToken(new()
        {
            ExpiringHours = 1,
            Payload = new Dictionary<string, string>
            {
                {"email", user.Email.Value}
            }
        });
    }

    /// <summary>
    /// Создание токена обновления.
    /// </summary>
    /// <param name="jwtProvider">Сервис работы с токенами.</param>
    /// <param name="ip">IP-адрес пользователя, с которого выполняется авторизация.</param>
    /// <returns>Токен обновления.</returns>
    internal static Result<RefreshToken> CreateRefreshToken(IJwtProvider jwtProvider,
        string? ip)
    {
        var refreshToken = jwtProvider.GenerateRefreshToken(new()
        {
            ExpiringHours = 720,
            Ip = ip
        });

        var factory = new RefreshTokenFactory(new(refreshToken.Token)
        {
            ExpirationDate = refreshToken.Expires,
            CreatedDate = refreshToken.Created,
            CreatedByIp = refreshToken.CreatedByIp
        });

        return factory.Create();
    }
}