using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Commands;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Queries;
using Pilotiv.AuthorizationAPI.Application.Shared.Services.OAuthVkProvider;
using Pilotiv.AuthorizationAPI.Infrastructure.Options;
using Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Context;
using Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Migrations;
using Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Repositories.Commands;
using Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Repositories.Queries;
using Pilotiv.AuthorizationAPI.Infrastructure.Services.OAuth;

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
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        var services = builder.Services;
        
        services.AddScoped<IOAuthVkProvider, OAuthVkProvider>();

        services.AddSingleton<DbContext>();
        services.AddSingleton<DbMigration>();
        
        services.AddScoped<IUsersCommandsRepository, UsersCommandsRepository>();
        services.AddScoped<IUsersQueriesRepository, UsersQueriesRepository>();

        
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(configure => configure
                .AddPostgres()
                .WithGlobalConnectionString(GetConnectionString(builder.Configuration))
                .ScanIn(Assembly.GetExecutingAssembly())
                .For.Migrations())
            .AddLogging(configure => configure.AddFluentMigratorConsole())
            .BuildServiceProvider(false);

        return builder;
    }

    /// <summary>
    /// Получение строки соединения с базой данных.
    /// </summary>
    /// <param name="configuration">Конфигурация.</param>
    private static string GetConnectionString(IConfiguration configuration)
    {
        var section = configuration.GetSection(nameof(DbSettingsOptions.DbSettings));

        var host = section.GetValueOfSection(nameof(DbSettingsOptions.Host));
        var port = section.GetValueOfSection(nameof(DbSettingsOptions.Port));
        var database = section.GetValueOfSection(nameof(DbSettingsOptions.Database));
        var userId = section.GetValueOfSection(nameof(DbSettingsOptions.UserId));
        var password = section.GetValueOfSection(nameof(DbSettingsOptions.Password));

        return $"Host={host};Port={port};Database={database}; Username={userId}; Password={password};";
    }

    /// <summary>
    /// Получение значения раздела конфигурации.
    /// </summary>
    private static string GetValueOfSection(this IConfigurationSection section, string key)
    {
        return section.GetSection(key).Value ?? string.Empty;
    }
}