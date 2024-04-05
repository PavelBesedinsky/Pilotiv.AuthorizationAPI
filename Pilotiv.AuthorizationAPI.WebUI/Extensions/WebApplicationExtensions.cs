using Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Context;
using Pilotiv.AuthorizationAPI.WebUI.Settings;
using Serilog;

namespace Pilotiv.AuthorizationAPI.WebUI.Extensions;

/// <summary>
/// Расширения <see cref="WebApplication"/>.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Задание пользовательских настроек для инициализации <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="webApplication"><see cref="WebApplication"/>.</param>
    /// <returns><see cref="WebApplication"/>.</returns>
    public static WebApplication ConfigureWebApplication(this WebApplication webApplication)
    {
        if (webApplication.Environment.IsDevelopment())
        {
            webApplication.UseOpenApi();
            webApplication.UseSwaggerUi();
        }

        if (webApplication.Environment.IsContainer())
        {
            webApplication.UseMiddleware<SwaggerAuthorizeMiddleware>();
            webApplication.UseOpenApi();
            webApplication.UseSwaggerUi();
        }
        
        webApplication.UseReDoc(ReDocSettings.Apply);

        webApplication.UseSerilogRequestLogging();
        webApplication.MapControllers();

        
        webApplication.Lifetime.ApplicationStarted.Register(OnApplicationStarted, webApplication.Services);

        return webApplication;
    }

    private static async void OnApplicationStarted(object? provider)
    {
        if (provider is not ServiceProvider serviceProvider)
        {
            return;
        }

        await InitDataBaseAsync(serviceProvider);
    }


    private static Task InitDataBaseAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<DataContext>();
        return context.InitAsync();
    }
}