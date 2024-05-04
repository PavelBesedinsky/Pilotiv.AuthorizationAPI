namespace Pilotiv.AuthorizationAPI.WebUI.Dtos.AuthController;

/// <summary>
/// Объект переноса данных запроса отзыва токена обновления.
/// </summary>
public class RevokeRefreshTokenRequest
{
    /// <summary>
    /// Токен обновления.
    /// </summary>
    public string? RefreshToken { get; init; }

    /// <summary>
    /// Причина отзыва токена.
    /// </summary>
    public string? Reason { get; init; }
}