using FluentResults;
using Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users.Dtos;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Entities;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

namespace Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users;

/// <summary>
/// Фабрика пользователей.
/// </summary>
public class UsersFabric : IAggregateFabric<User, UserId, Guid>
{
    private readonly UsersFabricUserPayload _value;

    /// <summary>
    /// Создание фабрики пользователей.
    /// </summary>
    /// <param name="value">Объект переноса данных информации о пользователе для фабрики пользователей.</param>
    public UsersFabric(UsersFabricUserPayload value)
    {
        _value = value;
    }

    /// <inheritdoc />
    public Result<User> Create()
    {
        List<IError> errors = new();

        // Получение хэша-пароля пользователя.
        UserPasswordHash? passwordHash = null;
        if (!string.IsNullOrWhiteSpace(_value.PasswordHash))
        {
            var getPasswordHashResult = UserPasswordHash.Create(_value.PasswordHash);
            if (getPasswordHashResult.IsFailed)
            {
                errors.AddRange(getPasswordHashResult.Errors);
            }

            passwordHash = getPasswordHashResult.ValueOrDefault;
        }

        // Получение адреса электронной почты пользователя.
        var getUserEmailResult = UserEmail.Create(_value.Email ?? string.Empty);
        if (getUserEmailResult.IsFailed)
        {
            errors.AddRange(getUserEmailResult.Errors);
        }

        var email = getUserEmailResult.ValueOrDefault;

        // Получение даты регистрации пользователя.
        var getUserRegistrationDate = UserRegistrationDate.Create(_value.RegistrationDate);
        if (getUserRegistrationDate.IsFailed)
        {
            errors.AddRange(getUserRegistrationDate.Errors);
        }

        var registrationDate = getUserRegistrationDate.ValueOrDefault;

        // Получение даты авторизации пользователя.
        var getUserAuthorizationDate = UserAuthorizationDate.Create(_value.AuthorizationDate);
        if (getUserAuthorizationDate.IsFailed)
        {
            errors.AddRange(getUserAuthorizationDate.Errors);
        }

        var authorizationDate = getUserAuthorizationDate.ValueOrDefault;

        UserLogin? login = null;
        if (!string.IsNullOrWhiteSpace(_value.Login))
        {
            // Получение логина пользователя.
            var getUserLoginResult = UserLogin.Create(_value.Login);
            if (getUserLoginResult.IsFailed)
            {
                errors.AddRange(getUserLoginResult.Errors);
            }

            login = getUserLoginResult.ValueOrDefault;
        }
        
        // Создание пользователя.
        var createUserResult = User.Create(passwordHash, email, registrationDate, authorizationDate, login);
        if (createUserResult.IsFailed)
        {
            errors.AddRange(createUserResult.Errors);
        }

        var user = createUserResult.ValueOrDefault;
        
        // Получение пользователя VK.
        VkUser? vkUser = null;
        if (_value.VkUser is not null)
        {
            var getVkUserResult = CreateVkUser(_value.VkUser);
            if (getVkUserResult.IsFailed)
            {
                errors.AddRange(getVkUserResult.Errors);
            }

            vkUser = getVkUserResult.ValueOrDefault;
        }
        
        // Добавление пользователя VK.
        if (vkUser is not null)
        {
            var addVkUserResult = user.AddVkUser(vkUser);
            if (addVkUserResult.IsFailed)
            {
                errors.AddRange(addVkUserResult.Errors);
            }
        }

        if (errors.Any())
        {
            return Result.Fail(errors);
        }

        return user;
    }

    /// <inheritdoc />
    public Result<User> Restore(Guid id)
    {
        List<IError> errors = new();

        // Получение хэша-пароля пользователя.
        UserPasswordHash? passwordHash = null;
        if (!string.IsNullOrWhiteSpace(_value.PasswordHash))
        {
            var getPasswordHashResult = UserPasswordHash.Create(_value.PasswordHash);
            if (getPasswordHashResult.IsFailed)
            {
                errors.AddRange(getPasswordHashResult.Errors);
            }

            passwordHash = getPasswordHashResult.ValueOrDefault;
        }

        // Получение адреса электронной почты пользователя.
        var getUserEmailResult = UserEmail.Create(_value.Email ?? string.Empty);
        if (getUserEmailResult.IsFailed)
        {
            errors.AddRange(getUserEmailResult.Errors);
        }

        var email = getUserEmailResult.ValueOrDefault;

        // Получение даты регистрации пользователя.
        var getUserRegistrationDate = UserRegistrationDate.Create(_value.RegistrationDate);
        if (getUserRegistrationDate.IsFailed)
        {
            errors.AddRange(getUserRegistrationDate.Errors);
        }

        var registrationDate = getUserRegistrationDate.ValueOrDefault;

        // Получение даты авторизации пользователя.
        var getUserAuthorizationDate = UserAuthorizationDate.Create(_value.AuthorizationDate);
        if (getUserAuthorizationDate.IsFailed)
        {
            errors.AddRange(getUserAuthorizationDate.Errors);
        }

        var authorizationDate = getUserAuthorizationDate.ValueOrDefault;

        // Получение логина пользователя.
        var getUserLoginResult = UserLogin.Create(_value.Login ?? string.Empty);
        if (getUserLoginResult.IsFailed)
        {
            errors.AddRange(getUserAuthorizationDate.Errors);
        }

        var login = getUserLoginResult.ValueOrDefault;

        // Получение пользователя VK.
        VkUser? vkUser = null;
        if (_value.VkUser is not null)
        {
            var getVkUserResult = RestoreVkUser(_value.VkUser);
            if (getVkUserResult.IsFailed)
            {
                errors.AddRange(getVkUserResult.Errors);
            }

            vkUser = getVkUserResult.ValueOrDefault;
        }

        if (errors.Any())
        {
            return Result.Fail(errors);
        }

        return User.Restore(UserId.Create(id), passwordHash, email, registrationDate, authorizationDate, login, vkUser);
    }

    /// <summary>
    /// Создание пользователя VK.
    /// </summary>
    /// <param name="value">Объект переноса данных информации о пользователе VK для фабрики пользователей.</param>
    /// <returns>Пользователь VK.</returns>
    private static Result<VkUser> CreateVkUser(UsersFabricVkUserPayload value)
    {
        var getVkInternalUserIdResult = VkInternalUserId.Create(value.InternalUserId ?? string.Empty);
        if (getVkInternalUserIdResult.IsFailed)
        {
            return getVkInternalUserIdResult.ToResult();
        }

        var vkInternalUserId = getVkInternalUserIdResult.ValueOrDefault;

        var getVkUserResult = VkUser.Create(vkInternalUserId);
        if (getVkUserResult.IsFailed)
        {
            return getVkUserResult.ToResult();
        }

        return getVkUserResult.ValueOrDefault;
    }

    /// <summary>
    /// Восстановление пользователя VK.
    /// </summary>
    /// <param name="value">Объект переноса данных информации о пользователе VK для фабрики пользователей.</param>
    /// <returns>Пользователь VK.</returns>
    private static Result<VkUser> RestoreVkUser(UsersFabricVkUserPayload value)
    {
        var getVkInternalUserIdResult = VkInternalUserId.Create(value.InternalUserId ?? string.Empty);
        if (getVkInternalUserIdResult.IsFailed)
        {
            return getVkInternalUserIdResult.ToResult();
        }

        var vkInternalUserId = getVkInternalUserIdResult.ValueOrDefault;

        var getVkUserResult = VkUser.Restore(VkUserId.Create(value.Id), vkInternalUserId);
        if (getVkUserResult.IsFailed)
        {
            return getVkUserResult.ToResult();
        }

        return getVkUserResult.ValueOrDefault;
    }
}