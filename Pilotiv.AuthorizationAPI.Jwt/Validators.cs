using System;
using Microsoft.IdentityModel.Tokens;

namespace Pilotiv.AuthorizationAPI.Jwt;

/// <summary>
/// Валидаторы.
/// </summary>
public static class Validators
{
    /// <summary>
    /// Выполнение валидации времени жизни токена.
    /// </summary>
    /// <param name="notBefore">Дата начала жизни токена.</param>
    /// <param name="expires">Дата конца жизни токена.</param>
    /// <param name="securityToken">Токен.</param>
    /// <param name="validationParameters">Параметры валидации.</param>
    public static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken,
        TokenValidationParameters validationParameters)
    {
        return expires != null && expires > DateTime.UtcNow;
    }
}