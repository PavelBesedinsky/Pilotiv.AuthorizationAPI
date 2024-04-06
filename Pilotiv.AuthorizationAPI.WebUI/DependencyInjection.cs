using Pilotiv.AuthorizationAPI.WebUI.Settings;

namespace Pilotiv.AuthorizationAPI.WebUI;

/// <summary>
/// Класс определения зависимостей слоя "Презентация"
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавление зависимостей слоя "Приложение"
    /// </summary>
    /// <param name="builder">Констуктор приложения.</param>
    /// <returns>Конструктор приложения.</returns>
    public static IHostApplicationBuilder AddPresentation(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddOpenApiDocument(OpenApiSettings.OpenApiDocument);

        services.AddScoped<SwaggerAuthorizeMiddleware>();
        
        return builder;
    }
}