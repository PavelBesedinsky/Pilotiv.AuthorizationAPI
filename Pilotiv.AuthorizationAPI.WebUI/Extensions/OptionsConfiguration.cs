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

        services.Configure<DbSettingsOptions>(dbSettingsOptions);
        return services;
    }
}