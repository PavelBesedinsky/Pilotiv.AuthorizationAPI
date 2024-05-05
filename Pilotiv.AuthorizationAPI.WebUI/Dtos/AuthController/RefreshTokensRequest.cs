namespace Pilotiv.AuthorizationAPI.WebUI.Dtos.AuthController;

/// <summary>
/// Объект переноса данных команды обновления токенов.
/// </summary>
public class RefreshTokensRequest
{
    /// <summary>
    /// Токен обновления.
    /// </summary>
    public string? RefreshToken { get; init; }
}