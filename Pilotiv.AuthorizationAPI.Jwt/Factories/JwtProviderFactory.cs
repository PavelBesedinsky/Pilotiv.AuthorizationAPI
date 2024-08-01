using Microsoft.Extensions.Options;
using Pilotiv.AuthorizationAPI.Jwt.Abstractions;
using Pilotiv.AuthorizationAPI.Jwt.ConfigurationOptions;
using Pilotiv.AuthorizationAPI.Jwt.Services;

namespace Pilotiv.AuthorizationAPI.Jwt.Factories;

/// <summary>
/// Фабрика сервиса работы с токенами.
/// </summary>
public static class JwtProviderFactory
{
    /// <summary>
    /// Создание сервиса работы с токенами.
    /// </summary>
    /// <param name="options">Настройки генерации и валидации ключей доступа.</param>
    /// <returns>Сервис работы стокенами.</returns>
    public static IJwtProvider CreateJwtProvider(IOptionsMonitor<AuthenticationKeysOption> options)
    {
        return new JwtProvider(options);
    }
}