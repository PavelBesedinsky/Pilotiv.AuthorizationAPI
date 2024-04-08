using FluentResults;
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
    /// Адрес электронной почты пользователя.
    /// </summary>
    public UserEmail? Email { get; }

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
    /// Создание пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="email">Адрес электронной почты пользователя.</param>
    /// <param name="registrationDate">Дата прегистрации пользователя.</param>
    /// <param name="authorizationDate">Дата авторизации пользователя.</param>
    /// <param name="login">Логин.</param>
    private User(UserId id, UserEmail email, UserRegistrationDate registrationDate,
        UserAuthorizationDate authorizationDate, UserLogin login) : base(id)
    {
        Email = email;
        RegistrationDate = registrationDate;
        AuthorizationDate = authorizationDate;
        Login = login;
    }

    /// <summary>
    /// Создание пользователя.
    /// </summary>
    /// <param name="email">Адрес электронной почты пользователя.</param>
    /// <param name="registrationDate">Дата прегистрации пользователя.</param>
    /// <param name="authorizationDate">Дата авторизации пользователя.</param>
    /// <param name="login">Логин.</param>
    private User(UserEmail email, UserRegistrationDate registrationDate, UserAuthorizationDate authorizationDate,
        UserLogin login) : this(UserId.Create(), email, registrationDate, authorizationDate, login)
    {
    }

    /// <summary>
    /// Создание пользователя.
    /// </summary>
    /// <param name="email">Адрес электронной почты пользователя.</param>
    /// <param name="registrationDate">Дата прегистрации пользователя.</param>
    /// <param name="authorizationDate">Дата авторизации пользователя.</param>
    /// <param name="login">Логин пользователя.</param>
    /// <returns>Пользователь.</returns>
    internal static Result<User> Create(UserEmail email, UserRegistrationDate registrationDate,
        UserAuthorizationDate authorizationDate, UserLogin login)
    {
        var user = new User(email, registrationDate, authorizationDate, login);

        user.AddDomainEvent(new UserCreatedDomainEvent(user));
        user.AddDomainEvent(new UserEmailChangedDomainEvent(user.Id, email));
        user.AddDomainEvent(new UserRegistrationDateChangedDomainEvent(user.Id, registrationDate));
        user.AddDomainEvent(new UserAuthorizationDateChanged(user.Id, authorizationDate));

        return user;
    }

    /// <summary>
    /// Восстановление пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="email">Адрес электронной почты пользователя.</param>
    /// <param name="registrationDate">Дата прегистрации пользователя.</param>
    /// <param name="authorizationDate">Дата авторизации пользователя.</param>
    /// <param name="login">Логин.</param>
    /// <returns>Пользователь.</returns>
    internal static Result<User> Restore(UserId userId, UserEmail email, UserRegistrationDate registrationDate,
        UserAuthorizationDate authorizationDate, UserLogin login)
    {
        return new User(userId, email, registrationDate, authorizationDate, login);
    }
}