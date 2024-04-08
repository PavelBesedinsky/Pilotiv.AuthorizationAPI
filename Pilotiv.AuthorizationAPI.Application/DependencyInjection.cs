using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Pilotiv.AuthorizationAPI.Application;

/// <summary>
/// Класс определения зависимостей слоя "Приложение"
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавление зависимостей слоя "Приложение"
    /// </summary>
    /// <param name="builder">Констуктор приложения.</param>
    /// <returns>Конструктор приложения.</returns>
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        // services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

        return builder;
    }
}