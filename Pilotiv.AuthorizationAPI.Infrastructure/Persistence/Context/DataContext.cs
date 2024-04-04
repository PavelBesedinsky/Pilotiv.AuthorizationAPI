using System.Data;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using Pilotiv.AuthorizationAPI.Infrastructure.Options;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Context;

/// <summary>
/// Контекст базы данных.
/// </summary>
public class DataContext
{
    private readonly DbSettingsOptions _dbSettingsOptions;

    /// <summary>
    /// Создание контекста базы данных.
    /// </summary>
    /// <param name="dbSettings">Настройки подключения к базе данных.</param>
    public DataContext(IOptions<DbSettingsOptions> dbSettings)
    {
        _dbSettingsOptions = dbSettings.Value;
    }

    /// <summary>
    /// Создание открытого соединения с базой данных.
    /// </summary>
    public IDbConnection CreateOpenedConnection()
    {
        var connection = CreateConnection();
        connection.Open();
        return connection;
    }

    /// <summary>
    /// Создание соединения с базой данных.
    /// </summary>
    private IDbConnection CreateConnection()
    {
        var connectionString =
            $"Host={_dbSettingsOptions.Host};Port={_dbSettingsOptions.Port};Database={_dbSettingsOptions.Database}; Username={_dbSettingsOptions.UserId}; Password={_dbSettingsOptions.Password};";
        return new NpgsqlConnection(connectionString);
    }

    /// <summary>
    /// Инициализация соединения.
    /// </summary>
    public async Task InitAsync()
    {
        await InitDatabaseAsync();
        
        using var connection = CreateConnection();
        connection.Open();
        var trx = connection.BeginTransaction();

        await InitTablesAsync(connection, trx);

        trx.Commit();
    }

    /// <summary>
    /// Инициализация базы данных.
    /// </summary>
    private async Task InitDatabaseAsync()
    {
        var connectionString =
            $"Host={_dbSettingsOptions.Host};Port={_dbSettingsOptions.Port};Database=postgres; Username={_dbSettingsOptions.UserId}; Password={_dbSettingsOptions.Password};";
        await using var connection = new NpgsqlConnection(connectionString);
        
        // create database if it doesn't exist
        var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{_dbSettingsOptions.Database}';";
        var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount);
        if (dbCount == 0)
        {
            var sql = $"CREATE DATABASE \"{_dbSettingsOptions.Database}\"";
            await connection.ExecuteAsync(sql);
        }
    }

    /// <summary>
    /// Инициализация таблиц.
    /// </summary>
    private static Task InitTablesAsync(IDbConnection connection, IDbTransaction trx)
    {
        return Task.CompletedTask;
    }
}