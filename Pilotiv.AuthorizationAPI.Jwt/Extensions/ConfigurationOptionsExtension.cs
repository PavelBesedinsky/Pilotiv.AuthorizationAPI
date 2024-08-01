using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pilotiv.AuthorizationAPI.Jwt.ConfigurationOptions;

namespace Pilotiv.AuthorizationAPI.Jwt.Extensions;

/// <summary>
/// Расширение добавления настроек.
/// </summary>
internal static class ConfigurationOptionsExtension
{
    /// <summary>
    /// Добавление настроек для генерации и валидации ключей доступа..
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация.</param>
    /// <returns>Коллекция сервисов.</returns>
    internal static IServiceCollection AddAuthenticationKeysOption(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthenticationKeysOption>(
            configuration.GetSection(AuthenticationKeysOption.AuthenticationKeys));
        
        return services;
    }
}