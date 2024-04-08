namespace Pilotiv.AuthorizationAPI.Infrastructure.Daos;

/// <summary>
/// Объект доступа данных пользователя.
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
    /// Логин пользователя.
    /// </summary>
    public string? Login { get; set; }
    
    /// <summary>
    /// Дата регистрации пользователя.
    /// </summary>
    public DateTime RegistrationDate { get; set; }
    
    /// <summary>
    /// Дата авторизации пользователя.
    /// </summary>
    public DateTime AuthorizationDate { get; set; }

    /// <summary>
    /// Получение признака, что пользователь является пользователем VK.
    /// </summary>
    public bool IsVkUser => VkUserId is not null;

    /// <summary>
    /// Идентификатор пользователя VK.
    /// </summary>
    public Guid? VkUserId { get; set; }
}