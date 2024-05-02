namespace Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users.Dtos;

/// <summary>
/// Объект переноса данных информации о пользователе VK для фабрики пользователей.
/// </summary>
public class UsersFactoryVkUserPayload
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Внутренний идентификатор пользователя в VK.
    /// </summary>
    public string? InternalUserId { get; set; }
}