namespace Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users.Dtos;

/// <summary>
/// Объект переноса данных информации о пользователе для фабрики пользователей.
/// </summary>
public class UsersFabricUserPayload
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Хэш-пароля пользователя.
    /// </summary>
    public string? PasswordHash { get; set; }

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
    /// Объект доступа данных пользователя VK.
    /// </summary>
    public UsersFabricVkUserPayload? VkUser { get; set; }
}