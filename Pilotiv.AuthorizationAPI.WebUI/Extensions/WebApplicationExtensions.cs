using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pilotiv.AuthorizationAPI.WebUI.Middlewares;
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
        webApplication.UseCors(builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
        webApplication.UseMiddleware<WebRootRedirectMiddleware>();
        
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

        webApplication.UseStaticFiles();
        
        webApplication.Lifetime.ApplicationStarted.Register(OnApplicationStarted, webApplication.Services);

        return webApplication;
    }

    private static async void OnApplicationStarted(object? provider)
    {
        if (provider is not ServiceProvider serviceProvider)
        {
            return;
        }
    }
}