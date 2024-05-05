namespace Pilotiv.AuthorizationAPI.Application.Shared.Options;

/// <summary>
/// Настройки для Authorization Code Flow для получения Access token пользователя.
/// </summary>
public class OAuthVkCredentialsOptions
{
    public const string OAuthVkCredentials = nameof(OAuthVkCredentials);

    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
    public string? RedirectUri { get; init; }
}