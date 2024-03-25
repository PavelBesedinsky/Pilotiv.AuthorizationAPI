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
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddOpenApiDocument(OpenApiSettings.OpenApiDocument);

        return services;
    }
}