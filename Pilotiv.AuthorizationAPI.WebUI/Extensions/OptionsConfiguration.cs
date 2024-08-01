using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pilotiv.AuthorizationAPI.Application.Shared.Options;
using Pilotiv.AuthorizationAPI.Infrastructure.Options;
using Pilotiv.AuthorizationAPI.Jwt.ConfigurationOptions;

namespace Pilotiv.AuthorizationAPI.WebUI.Extensions;

/// <summary>
/// Настройка опций.
/// </summary>
public static class OptionsConfiguration
{
    /// <summary>
    /// Настройка опций
    /// </summary>
    /// <param name="services">Службы.</param>
    /// <param name="webApplicationBuilder">Конструктор приложения..</param>
    public static IServiceCollection ConfigureOptions(this IServiceCollection services,
        WebApplicationBuilder webApplicationBuilder)
    {
        var dbSettingsOptions = webApplicationBuilder.Configuration.GetSection(DbSettingsOptions.DbSettings);
        var oAuthVkCredentialsOptions =
            webApplicationBuilder.Configuration.GetSection(OAuthVkCredentialsOptions.OAuthVkCredentials);
        var authenticationKeysOptions =
            webApplicationBuilder.Configuration.GetSection(AuthenticationKeysOption.AuthenticationKeys);

        services.Configure<DbSettingsOptions>(dbSettingsOptions);
        services.Configure<OAuthVkCredentialsOptions>(oAuthVkCredentialsOptions);
        services.Configure<AuthenticationKeysOption>(authenticationKeysOptions);

        return services;
    }
}