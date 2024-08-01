namespace Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users.Dtos;

/// <summary>
/// Объект переноса данных информации о пользователе для фабрики пользователей.
/// </summary>
public class UsersFactoryUserPayload
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Хэш-пароля пользователя.
    /// </summary>
    public string? PasswordHash { get; init; }

    /// <summary>
    /// Соль.
    /// </summary>
    public string? Salt { get; init; }
    
    /// <summary>
    /// Электронный адрес пользователя.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Логин пользователя.
    /// </summary>
    public string? Login { get; init; }

    /// <summary>
    /// Дата регистрации пользователя.
    /// </summary>
    public DateTime RegistrationDate { get; init; }

    /// <summary>
    /// Дата авторизации пользователя.
    /// </summary>
    public DateTime AuthorizationDate { get; init; }

    /// <summary>
    /// Объект доступа данных пользователя VK.
    /// </summary>
    public UsersFactoryVkUserPayload? VkUser { get; init; }
    
    /// <summary>
    /// Объекты переноса данных информации о токенах обновления.
    /// </summary>
    public List<UsersFactoryRefreshTokenPayload> RefreshTokens { get; init; } = new();
}