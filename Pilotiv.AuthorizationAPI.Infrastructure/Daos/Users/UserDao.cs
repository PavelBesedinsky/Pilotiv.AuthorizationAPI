﻿namespace Pilotiv.AuthorizationAPI.Infrastructure.Daos.Users;

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
    /// Объект доступа данных пользователя VK.
    /// </summary>
    public VkUserDao? VkUser { get; set; }
}