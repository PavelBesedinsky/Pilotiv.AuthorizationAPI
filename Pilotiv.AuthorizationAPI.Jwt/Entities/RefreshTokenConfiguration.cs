using System;

namespace Pilotiv.AuthorizationAPI.Jwt.Entities;

/// <summary>
/// Объект переноса данных настроек токена обновления.
/// </summary>
public class RefreshTokenConfiguration
{
    /// <summary>
    /// Дата и время, когда токен станет невалидным.
    /// </summary>
    public DateTime Expires { get; init; }

    /// <summary>
    /// IP пользователя, который вызывает изменение токена.
    /// </summary>
    public string? Ip { get; init; }
}