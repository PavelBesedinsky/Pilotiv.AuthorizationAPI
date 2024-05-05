using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Migrations;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Extensions;

/// <summary>
/// Управление миграцией БД.
/// </summary>
public static class MigrationManager
{
    /// <summary>
    /// Выполнение миграции БД.
    /// </summary>
    public static IHost MigrateDb(this IHost host, ILogger? logger)
    {
        using var scope = host.Services.CreateScope();

        try
        {
            var dbMigration = scope.ServiceProvider.GetRequiredService<DbMigration>();
            var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

            _ = dbMigration.CreateDbAsync();

            migrationRunner.ListMigrations();
            migrationRunner.MigrateUp();
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "Error duration migration");
            throw;
        }

        return host;
    }
}