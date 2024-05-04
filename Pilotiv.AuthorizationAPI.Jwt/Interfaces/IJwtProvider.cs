using System.Collections.Generic;
using System.Security.Claims;
using Pilotiv.AuthorizationAPI.Jwt.Entities;

namespace Pilotiv.AuthorizationAPI.Jwt.Interfaces;

/// <summary>
/// Интерфейс работы с JWT.
/// </summary>
internal interface IJwtProvider
{
    /// <summary>
    /// Создание токен доступа.
    /// </summary>
    /// <param name="configuration">Конфигурация токена.</param>
    /// <returns>Токен доступа.</returns>
    internal string GenerateAccessToken(AccessTokenConfiguration configuration);
    
    /// <summary>
    /// Валидация токена доступа.
    /// </summary>
    /// <param name="token">Токен доступа.</param>
    /// <returns>Клеймы.</returns>
    internal IEnumerable<Claim> ValidateAccessToken(string token);
    
    /// <summary>
    /// Создание токена обновления.
    /// </summary>
    /// <param name="configuration">Объект переноса данных настроек токена обновления.</param>
    /// <returns>Сущность токена обновления.</returns>
    internal RefreshToken GenerateRefreshToken(RefreshTokenConfiguration configuration);
}