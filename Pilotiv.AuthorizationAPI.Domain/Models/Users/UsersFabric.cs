using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users;

/// <summary>
/// Фабрика пользователей.
/// </summary>
public class UsersFabric : AggregateFabric<User, UserId, Guid>
{
    private readonly string _email;
    private readonly DateTime _registrationDate;
    private readonly DateTime _authorizationDate;
    private readonly string _login;

    /// <summary>
    /// Создание фабрики пользователей.
    /// </summary>
    /// <param name="email">Адрес электронной почты пользователя.</param>
    /// <param name="registrationDate">Дата регистрации пользователя.</param>
    /// <param name="authorizationDate">Дата авторизации пользователя.</param>
    /// <param name="login">Логин.</param>
    public UsersFabric(string email, DateTime registrationDate, DateTime authorizationDate, string login)
    {
        _email = email;
        _registrationDate = registrationDate;
        _authorizationDate = authorizationDate;
        _login = login;
    }

    /// <inheritdoc />
    public override Result<User> Create()
    {
        List<IError> errors = new();

        var getUserEmailResult = UserEmail.Create(_email);
        if (getUserEmailResult.IsFailed)
        {
            errors.AddRange(getUserEmailResult.Errors);
        }

        var email = getUserEmailResult.ValueOrDefault;

        var getUserRegistrationDate = UserRegistrationDate.Create(_registrationDate);
        if (getUserRegistrationDate.IsFailed)
        {
            errors.AddRange(getUserRegistrationDate.Errors);
        }

        var registrationDate = getUserRegistrationDate.ValueOrDefault;

        var getUserAuthorizationDate = UserAuthorizationDate.Create(_authorizationDate);
        if (getUserAuthorizationDate.IsFailed)
        {
            errors.AddRange(getUserAuthorizationDate.Errors);
        }

        var authorizationDate = getUserAuthorizationDate.ValueOrDefault;

        var getUserLoginResult = UserLogin.Create(_login);
        if (getUserLoginResult.IsFailed)
        {
            errors.AddRange(getUserAuthorizationDate.Errors);
        }

        var login = getUserLoginResult.ValueOrDefault;

        if (errors.Any())
        {
            return Result.Fail(errors);
        }

        return User.Create(email, registrationDate, authorizationDate, login);
    }

    /// <inheritdoc />
    public override Result<User> Restore(Guid id)
    {
        List<IError> errors = new();

        var getUserEmailResult = UserEmail.Create(_email);
        if (getUserEmailResult.IsFailed)
        {
            errors.AddRange(getUserEmailResult.Errors);
        }

        var email = getUserEmailResult.ValueOrDefault;

        var getUserRegistrationDate = UserRegistrationDate.Create(_registrationDate);
        if (getUserRegistrationDate.IsFailed)
        {
            errors.AddRange(getUserRegistrationDate.Errors);
        }

        var registrationDate = getUserRegistrationDate.ValueOrDefault;

        var getUserAuthorizationDate = UserAuthorizationDate.Create(_authorizationDate);
        if (getUserAuthorizationDate.IsFailed)
        {
            errors.AddRange(getUserAuthorizationDate.Errors);
        }

        var authorizationDate = getUserAuthorizationDate.ValueOrDefault;

        var getUserLoginResult = UserLogin.Create(_login);
        if (getUserLoginResult.IsFailed)
        {
            errors.AddRange(getUserAuthorizationDate.Errors);
        }

        var login = getUserLoginResult.ValueOrDefault;

        if (errors.Any())
        {
            return Result.Fail(errors);
        }

        return User.Restore(UserId.Create(id), email, registrationDate, authorizationDate, login);
    }
}