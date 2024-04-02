using Pilotiv.AuthorizationAPI.Application.Shared.Options;
using Pilotiv.AuthorizationAPI.Infrastructure.Options;

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
        
        services.Configure<DbSettingsOptions>(dbSettingsOptions);
        services.Configure<OAuthVkCredentialsOptions>(oAuthVkCredentialsOptions);

        return services;
    }
}