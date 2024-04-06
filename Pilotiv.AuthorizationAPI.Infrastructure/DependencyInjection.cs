using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pilotiv.AuthorizationAPI.Infrastructure.Options;
using Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Context;
using Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Migrations;

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
        var services = builder.Services;

        services.AddSingleton<DbContext>();
        services.AddSingleton<DbMigration>();

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

    private static string GetValueOfSection(this IConfigurationSection section, string key)
    {
        return section.GetSection(key).Value ?? string.Empty;
    }
}