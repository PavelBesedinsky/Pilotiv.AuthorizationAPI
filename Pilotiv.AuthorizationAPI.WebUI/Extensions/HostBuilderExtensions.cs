using Serilog;

namespace Pilotiv.AuthorizationAPI.WebUI.Extensions;

/// <summary>
/// Расширения <see cref="IHostBuilder"/>.
/// </summary>
public static class HostBuilderExtensions
{
    /// <summary>
    /// Задание пользовательских настроек для инициализации <see cref="IHostBuilder"/>.
    /// </summary>
    /// <param name="builder"><see cref="IHostBuilder"/>.</param>
    /// <returns><see cref="IHostBuilder"/>.</returns>
    public static IHostBuilder UseHostExtensions(this IHostBuilder builder)
    {
        builder.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services));
        
        return builder;
    }    
}