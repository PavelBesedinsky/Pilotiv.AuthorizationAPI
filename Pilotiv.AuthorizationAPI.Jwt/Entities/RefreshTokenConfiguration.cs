namespace Pilotiv.AuthorizationAPI.Jwt.Entities;

/// <summary>
/// Объект переноса данных настроек токена обновления.
/// </summary>
public class RefreshTokenConfiguration
{
    /// <summary>
    /// Количество часов, после истечения которых токен станет невалидным.
    /// </summary>
    /// <remarks>По умолчанию 1 месяц = 720 часов.</remarks>
    public int ExpiringHours { get; init; } = 720;

    /// <summary>
    /// IP пользователя, который вызывает изменение токена.
    /// </summary>
    public string? Ip { get; init; }
}