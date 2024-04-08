namespace Pilotiv.AuthorizationAPI.Infrastructure.Daos;

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
    public int VkUserId { get; set; }

    /// <summary>
    /// Идентификатор пользователя сервиса.
    /// </summary>
    public Guid UserId { get; init; }
}