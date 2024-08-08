using System.Data;
using Microsoft.Extensions.Options;
using Npgsql;
using Pilotiv.AuthorizationAPI.Infrastructure.Options;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Context;

/// <summary>
/// Контекст базы данных.
/// </summary>
public class DbContext
{
    private readonly DbSettingsOptions _dbSettingsOptions;

    /// <summary>
    /// Создание контекста базы данных.
    /// </summary>
    /// <param name="dbSettings">Настройки подключения к базе данных.</param>
    public DbContext(IOptions<DbSettingsOptions> dbSettings)
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
    /// PostgreSQL Connection String.
    /// </summary>
    private string SqlConnection =>
        $"Host={_dbSettingsOptions.Host};Port={_dbSettingsOptions.Port};Database={_dbSettingsOptions.Database}; Username={_dbSettingsOptions.UserId}; Password={_dbSettingsOptions.Password};";
    
    /// <summary>
    /// Создание соединения с БД.
    /// </summary>
    private IDbConnection CreateConnection() => new NpgsqlConnection(SqlConnection);
}