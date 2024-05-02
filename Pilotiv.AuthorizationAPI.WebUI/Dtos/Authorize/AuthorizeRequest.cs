namespace Pilotiv.AuthorizationAPI.WebUI.Dtos.Authorize;

/// <summary>
/// Объект переноса данных команды авторизации.
/// </summary>
public class AuthorizeRequest
{
    /// <summary>
    /// Логин
    /// </summary>
    public string? Login { get; init; }

    /// <summary>
    /// Пароль
    /// </summary>
    public string? Password { get; init; }
}