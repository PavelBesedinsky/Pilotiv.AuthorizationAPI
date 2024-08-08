using System;
using System.Collections.Generic;

namespace Pilotiv.AuthorizationAPI.Jwt.Entities;

/// <summary>
/// Объект переноса данных настроек Access Token.
/// </summary>
public class AccessTokenConfiguration
{
    /// <summary>
    /// Дата и время, раньше которого токен является невалидным.
    /// </summary>
    public DateTime NotBefore { get; init; }

    /// <summary>
    /// Дата и время, когда токен станет невалидным.
    /// </summary>
    public DateTime Expires { get; init; }

    /// <summary>
    /// Клеймы токены.
    /// </summary>
    public Dictionary<string, string> Claims { get; init; } = new();
}