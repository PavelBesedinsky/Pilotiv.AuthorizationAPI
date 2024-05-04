using System.Collections.Generic;

namespace Pilotiv.AuthorizationAPI.Jwt.Entities;

/// <summary>
/// Объект переноса данных настроек Access Token.
/// </summary>
public class AccessTokenConfiguration
{
    /// <summary>
    /// Количество часов, через которое истекает токен.
    /// </summary>
    /// <remarks>По умолчанию 7 дней = 168 часов.</remarks>
    public int ExpiringHours { get; init; } = 168;

    /// <summary>
    /// Тело токена.
    /// </summary>
    /// <remarks>Включает в себя Claims.</remarks>
    public Dictionary<string, string> Payload { get; init; } = new();
}