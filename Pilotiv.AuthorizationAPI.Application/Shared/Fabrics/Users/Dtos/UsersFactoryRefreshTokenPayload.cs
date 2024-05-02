namespace Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users.Dtos;

/// <summary>
/// Объект переноса данных информации о токене обновления для фабрики пользователей.
/// </summary>
public class UsersFactoryRefreshTokenPayload
{
    /// <summary>
    /// Создание объекта переноса данных информации о токене обновления для фабрики пользователей.
    /// </summary>
    /// <param name="id">Идентификатор токена обновления.</param>
    public UsersFactoryRefreshTokenPayload(string id)
    {
        Id = id;
    }

    /// <summary>
    /// Идентификатор токена.
    /// </summary>
    public string Id { get; }
    
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
    public UsersFactoryRefreshTokenPayload? ReplacingToken { get; set; }
}