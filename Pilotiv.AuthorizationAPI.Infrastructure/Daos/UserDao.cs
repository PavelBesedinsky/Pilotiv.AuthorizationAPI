namespace Pilotiv.AuthorizationAPI.Infrastructure.Daos;

/// <summary>
/// Объек доступа данных пользователя.
/// </summary>
public class UserDao
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Электронный адрес пользователя.
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Получение признака, что пользователь является пользователем VK.
    /// </summary>
    public bool IsVkUser => VkUserId is not null;

    /// <summary>
    /// Идентификатор пользователя VK.
    /// </summary>
    public Guid? VkUserId { get; set; }
}