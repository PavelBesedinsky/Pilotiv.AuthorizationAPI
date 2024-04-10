using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Entities;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users;

/// <summary>
/// Пользователь.
/// </summary>
public class User : AggregateRoot<UserId, Guid>
{
    /// <summary>
    /// Хэш-пароля пользователя.
    /// </summary>
    public UserPasswordHash? PasswordHash { get; }

    /// <summary>
    /// Адрес электронной почты пользователя.
    /// </summary>
    public UserEmail Email { get; }

    /// <summary>
    /// Логин пользователя.
    /// </summary>
    public UserLogin Login { get; }

    /// <summary>
    /// Дата регистрации пользователя.
    /// </summary>
    public UserRegistrationDate RegistrationDate { get; }

    /// <summary>
    /// Дата авторизации пользователя.
    /// </summary>
    public UserAuthorizationDate AuthorizationDate { get; }

    /// <summary>
    /// Пользователь VK.
    /// </summary>
    public VkUser? VkUser { get; }

    /// <summary>
    /// Создание пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="passwordHash">Хэш-пароля пользователя.</param>
    /// <param name="email">Адрес электронной почты пользователя.</param>
    /// <param name="registrationDate">Дата прегистрации пользователя.</param>
    /// <param name="authorizationDate">Дата авторизации пользователя.</param>
    /// <param name="login">Логин.</param>
    /// <param name="vkUser">Пользователь VK.</param>
    private User(UserId id, UserPasswordHash? passwordHash, UserEmail email, UserRegistrationDate registrationDate,
        UserAuthorizationDate authorizationDate, UserLogin login, VkUser? vkUser) : base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
        RegistrationDate = registrationDate;
        AuthorizationDate = authorizationDate;
        Login = login;
    }

    /// <summary>
    /// Создание пользователя.
    /// </summary>
    /// <param name="passwordHash">Хэш-пароля пользователя.</param>
    /// <param name="email">Адрес электронной почты пользователя.</param>
    /// <param name="registrationDate">Дата прегистрации пользователя.</param>
    /// <param name="authorizationDate">Дата авторизации пользователя.</param>
    /// <param name="login">Логин.</param>
    /// <param name="vkUser">Пользователь VK.</param>
    private User(UserPasswordHash? passwordHash, UserEmail email, UserRegistrationDate registrationDate,
        UserAuthorizationDate authorizationDate, UserLogin login, VkUser? vkUser) : this(UserId.Create(), passwordHash,
        email, registrationDate, authorizationDate, login, vkUser)
    {
    }

    /// <summary>
    /// Создание пользователя.
    /// </summary>
    /// <param name="userPasswordHash">Хэш-пароля пользователя.</param>
    /// <param name="email">Адрес электронной почты пользователя.</param>
    /// <param name="registrationDate">Дата прегистрации пользователя.</param>
    /// <param name="authorizationDate">Дата авторизации пользователя.</param>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="vkUser">Пользователь VK.</param>
    /// <returns>Пользователь.</returns>
    public static Result<User> Create(UserPasswordHash? userPasswordHash, UserEmail email,
        UserRegistrationDate registrationDate, UserAuthorizationDate authorizationDate, UserLogin login, VkUser? vkUser)
    {
        var user = new User(userPasswordHash, email, registrationDate, authorizationDate, login, vkUser);

        user.AddDomainEvent(new UserCreatedDomainEvent(user.Id));
        if (userPasswordHash is not null)
        {
            user.AddDomainEvent(new UserPasswordHashChangedDomainEvent(user.Id, userPasswordHash));
        }

        user.AddDomainEvent(new UserEmailChangedDomainEvent(user.Id, email));
        user.AddDomainEvent(new UserRegistrationDateChangedDomainEvent(user.Id, registrationDate));
        user.AddDomainEvent(new UserAuthorizationDateChanged(user.Id, authorizationDate));
        user.AddDomainEvent(new UserLoginChangedDomainEvent(user.Id, login));
        if (vkUser is not null)
        {
            user.AddDomainEvent(new UserVkUserChangedDomainEvent(user.Id, vkUser));
        }

        return user;
    }

    /// <summary>
    /// Восстановление пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="userPasswordHash">Хэш-пароля пользователя.</param>
    /// <param name="email">Адрес электронной почты пользователя.</param>
    /// <param name="registrationDate">Дата прегистрации пользователя.</param>
    /// <param name="authorizationDate">Дата авторизации пользователя.</param>
    /// <param name="login">Логин.</param>
    /// <param name="vkUser">Пользователь VK.</param>
    /// <returns>Пользователь.</returns>
    public static Result<User> Restore(UserId userId, UserPasswordHash? userPasswordHash, UserEmail email,
        UserRegistrationDate registrationDate, UserAuthorizationDate authorizationDate, UserLogin login, VkUser? vkUser)
    {
        return new User(userId, userPasswordHash, email, registrationDate, authorizationDate, login, vkUser);
    }
}