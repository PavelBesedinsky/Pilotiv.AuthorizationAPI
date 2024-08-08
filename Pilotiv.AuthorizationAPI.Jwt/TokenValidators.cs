using System;
using Microsoft.IdentityModel.Tokens;

namespace Pilotiv.AuthorizationAPI.Jwt;

/// <summary>
/// Валидаторы.
/// </summary>
public static class TokenValidators
{
    /// <summary>
    /// Валидация времени жизни токена.
    /// </summary>
    /// <param name="notBefore">Дата начала жизни токена.</param>
    /// <param name="expires">Дата конца жизни токена.</param>
    /// <param name="securityToken">Токен.</param>
    /// <param name="validationParameters">Параметры валидации.</param>
    public static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken,
        TokenValidationParameters validationParameters)
    {
        var utcNow = DateTime.UtcNow;

        var isNotBeforeValid = notBefore is null || utcNow > notBefore;
        var isExpiresValid = expires is null || utcNow < expires;
        
        return isNotBeforeValid && isExpiresValid;
    }
}