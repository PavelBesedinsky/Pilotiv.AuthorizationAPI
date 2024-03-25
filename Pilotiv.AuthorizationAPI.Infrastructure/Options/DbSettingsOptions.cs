namespace Pilotiv.AuthorizationAPI.Infrastructure.Options;

/// <summary>
/// Настройки подключения к базе данных.
/// </summary>
public class DbSettingsOptions
{
    public const string DbSettings = nameof(DbSettings);

    public string Server { get; init; } = string.Empty;
    public string Database { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}