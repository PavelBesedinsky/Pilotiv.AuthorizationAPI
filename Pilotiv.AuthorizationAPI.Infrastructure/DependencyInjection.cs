using Microsoft.Extensions.DependencyInjection;
using Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Context;

namespace Pilotiv.AuthorizationAPI.Infrastructure;

/// <summary>
/// Класс определения зависимостей слоя "Инфраструктура".
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавление зависимостей слоя "Инфраструктура"
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<DataContext>();
        
        return services;
    }
}