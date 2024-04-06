using Dapper;
using Microsoft.Extensions.Options;
using Pilotiv.AuthorizationAPI.Infrastructure.Options;
using Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Context;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Migrations;

/// <summary>
/// Миграция базы данных.
/// </summary>
public class DbMigration
{
    private readonly DbContext _context;
    private readonly DbSettingsOptions _dbSettings;

    public DbMigration(DbContext context, IOptions<DbSettingsOptions> options)
    {
        _context = context;
        _dbSettings = options.Value;
    }

    /// <summary>
    /// Создание базы данных.
    /// </summary>
    public async Task CreateDbAsync()
    {
        using var connection = _context.CreateOpenedMasterConnection();
        
        var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{_dbSettings.Database}';";
        var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount);
        if (dbCount is 0)
        {
            var sql = $"CREATE DATABASE \"{_dbSettings.Database}\"";
            await connection.ExecuteAsync(sql);
        }
    }
}