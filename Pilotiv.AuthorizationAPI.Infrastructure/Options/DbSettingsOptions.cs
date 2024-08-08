namespace Pilotiv.AuthorizationAPI.Infrastructure.Options;

/// <summary>
/// Настройки подключения к базе данных.
/// </summary>
public class DbSettingsOptions
{
    public const string DbSettings = nameof(DbSettings);

    public string Host { get; init; } = string.Empty;
    public int Port { get; init; } = 80;
    public string Database { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}