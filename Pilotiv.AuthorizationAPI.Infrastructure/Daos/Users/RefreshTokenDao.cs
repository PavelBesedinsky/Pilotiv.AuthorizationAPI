namespace Pilotiv.AuthorizationAPI.Infrastructure.Daos.Users;

/// <summary>
/// Объект доступа данных токена обновления.
/// </summary>
public class RefreshTokenDao
{
    /// <summary>
    /// Идентификатор токена.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Пользователь.
    /// </summary>
    public UserDao? User { get; set; }

    /// <summary>
    /// Дата истечения токена.
    /// </summary>
    public DateTime ExpirationDate { get; set; }

    /// <summary>
    /// Дата создания токена.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Дата отзыва токена.
    /// </summary>
    public DateTime RevokedDate { get; set; }

    /// <summary>
    /// IP-адрес пользователя, запрашивающего создание токена.
    /// </summary>
    public string? CreatedByIp { get; set; }

    /// <summary>
    /// IP-адрес пользователя, запрашивающего отзыв токена.
    /// </summary>
    public string? RevokedByIp { get; set; }

    /// <summary>
    /// Причина отзыва токена.
    /// </summary>
    public string? RevokeReason { get; set; }

    /// <summary>
    /// Токен обновления, заменяющий текущий токен.
    /// </summary>
    public RefreshTokenDao? ReplacingToken { get; set; }
}