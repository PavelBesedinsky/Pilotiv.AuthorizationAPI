using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Commands;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Queries;
using Pilotiv.AuthorizationAPI.Application.Shared.Services;
using Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Context;
using Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Repositories.Commands;
using Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Repositories.Queries;
using Pilotiv.AuthorizationAPI.Infrastructure.Services.OAuth;
using Pilotiv.AuthorizationAPI.Infrastructure.Services.Security;

namespace Pilotiv.AuthorizationAPI.Infrastructure;

/// <summary>
/// Класс определения зависимостей слоя "Инфраструктура".
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавление зависимостей слоя "Инфраструктура"
    /// </summary>
    /// <param name="builder">Констуктор приложения.</param>
    /// <returns>Конструктор приложения.</returns>
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        // Включается для согласования названий переменных объектов доступа и столбцов базы данных.
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        var services = builder.Services;

        services.AddSingleton<DbContext>();
        
        services.AddScoped<IOAuthVkProvider, OAuthVkProvider>();
        services.AddScoped<IPasswordProvider, PasswordProvider>();

        services.AddScoped<IUsersCommandsRepository, UsersCommandsRepository>();
        services.AddScoped<IUsersQueriesRepository, UsersQueriesRepository>();
        
        return builder;
    }
}