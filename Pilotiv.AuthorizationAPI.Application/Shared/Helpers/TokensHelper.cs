using FluentResults;
using Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;
using Pilotiv.AuthorizationAPI.Jwt.Abstractions;
using Pilotiv.AuthorizationAPI.Jwt.Entities;
using RefreshToken = Pilotiv.AuthorizationAPI.Domain.Models.Users.Entities.RefreshToken;

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
        AccessTokenConfiguration configuration = new()
        {
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(15),
            Claims = new()
            {
                {"email", user.Email.Value}
            }
        };
        
        return jwtProvider.GenerateAccessToken(configuration);
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
        RefreshTokenConfiguration configuration = new()
        {
            Expires = DateTime.UtcNow.AddDays(1),
            Ip = ip
        };
        
        var refreshToken = jwtProvider.GenerateRefreshToken(configuration);

        var factory = new RefreshTokenFactory(new(refreshToken.Token)
        {
            ExpirationDate = refreshToken.Expires,
            CreatedDate = refreshToken.Created,
            CreatedByIp = refreshToken.CreatedByIp
        });

        return factory.Create();
    }
}