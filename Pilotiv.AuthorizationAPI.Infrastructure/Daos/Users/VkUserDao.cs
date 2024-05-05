namespace Pilotiv.AuthorizationAPI.Infrastructure.Daos.Users;

/// <summary>
/// Объект доступа данных пользователя VK.
/// </summary>
public class VkUserDao
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Внутренний идентификатор пользователя в VK.
    /// </summary>
    public string? InternalId { get; set; }
}