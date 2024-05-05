namespace Pilotiv.AuthorizationAPI.Jwt.ConfigurationOptions;

/// <summary>
/// Опция для генерации и валидации ключей доступа.
/// </summary>
public class AuthenticationKeysOption
{
    /// <summary>
    /// Наименование раздела.
    /// </summary>
    public const string AuthenticationKeys = nameof(AuthenticationKeys);

    /// <summary>
    /// Публичный ключ.
    /// </summary>
    public string? PublicKey { get; init; }

    /// <summary>
    /// Приватный ключ.
    /// </summary>
    public string? PrivateKey { get; init; }
}