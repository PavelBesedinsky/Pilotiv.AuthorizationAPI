namespace Pilotiv.AuthorizationAPI.Application.Shared.Dtos;

/// <summary>
/// Токен обновления.
/// </summary>
public class RefreshTokenPayload
{
    /// <summary>
    /// Токен.
    /// </summary>
    public string? Token { get; init; }

    /// <summary>
    /// Дата истечения.
    /// </summary>
    public DateTime Expires { get; init;}
}