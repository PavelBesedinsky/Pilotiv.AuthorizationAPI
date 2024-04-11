using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Entities;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Infrastructure.Daos.Users;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Fabrics.Users;

/// <summary>
/// Фабрика пользователей.
/// </summary>
public class UsersFabric : IAggregateFabric<User, UserId, Guid>
{
    private readonly UserDao _userDao;

    /// <summary>
    /// Создание фабрики пользователей.
    /// </summary>
    /// <param name="userDao">Объект доступа данных пользователя.</param>
    public UsersFabric(UserDao userDao)
    {
        _userDao = userDao;
    }

    /// <inheritdoc />
    public Result<User> Create()
    {
        List<IError> errors = new();

        // Получение хэша-пароля пользователя.
        UserPasswordHash? passwordHash = null;
        if (!string.IsNullOrWhiteSpace(_userDao.PasswordHash))
        {
            var getPasswordHashResult = UserPasswordHash.Create(_userDao.PasswordHash);
            if (getPasswordHashResult.IsFailed)
            {
                errors.AddRange(getPasswordHashResult.Errors);
            }

            passwordHash = getPasswordHashResult.ValueOrDefault;
        }

        // Получение адреса электронной почты пользователя.
        var getUserEmailResult = UserEmail.Create(_userDao.Email);
        if (getUserEmailResult.IsFailed)
        {
            errors.AddRange(getUserEmailResult.Errors);
        }

        var email = getUserEmailResult.ValueOrDefault;

        // Получение даты регистрации пользователя.
        var getUserRegistrationDate = UserRegistrationDate.Create(_userDao.RegistrationDate);
        if (getUserRegistrationDate.IsFailed)
        {
            errors.AddRange(getUserRegistrationDate.Errors);
        }

        var registrationDate = getUserRegistrationDate.ValueOrDefault;

        // Получение даты авторизации пользователя.
        var getUserAuthorizationDate = UserAuthorizationDate.Create(_userDao.AuthorizationDate);
        if (getUserAuthorizationDate.IsFailed)
        {
            errors.AddRange(getUserAuthorizationDate.Errors);
        }

        var authorizationDate = getUserAuthorizationDate.ValueOrDefault;

        // Получение логина пользователя.
        var getUserLoginResult = UserLogin.Create(_userDao.Login);
        if (getUserLoginResult.IsFailed)
        {
            errors.AddRange(getUserAuthorizationDate.Errors);
        }

        var login = getUserLoginResult.ValueOrDefault;

        // Получение пользователя VK.
        VkUser? vkUser = null;
        if (_userDao.VkUser is not null)
        {
            var getVkUserResult = CreateVkUser(_userDao.VkUser);
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

        return User.Create(passwordHash, email, registrationDate, authorizationDate, login, vkUser);
    }

    /// <inheritdoc />
    public Result<User> Restore(Guid id)
    {
        List<IError> errors = new();

        // Получение хэша-пароля пользователя.
        UserPasswordHash? passwordHash = null;
        if (!string.IsNullOrWhiteSpace(_userDao.PasswordHash))
        {
            var getPasswordHashResult = UserPasswordHash.Create(_userDao.PasswordHash);
            if (getPasswordHashResult.IsFailed)
            {
                errors.AddRange(getPasswordHashResult.Errors);
            }

            passwordHash = getPasswordHashResult.ValueOrDefault;
        }

        // Получение адреса электронной почты пользователя.
        var getUserEmailResult = UserEmail.Create(_userDao.Email);
        if (getUserEmailResult.IsFailed)
        {
            errors.AddRange(getUserEmailResult.Errors);
        }

        var email = getUserEmailResult.ValueOrDefault;

        // Получение даты регистрации пользователя.
        var getUserRegistrationDate = UserRegistrationDate.Create(_userDao.RegistrationDate);
        if (getUserRegistrationDate.IsFailed)
        {
            errors.AddRange(getUserRegistrationDate.Errors);
        }

        var registrationDate = getUserRegistrationDate.ValueOrDefault;

        // Получение даты авторизации пользователя.
        var getUserAuthorizationDate = UserAuthorizationDate.Create(_userDao.AuthorizationDate);
        if (getUserAuthorizationDate.IsFailed)
        {
            errors.AddRange(getUserAuthorizationDate.Errors);
        }

        var authorizationDate = getUserAuthorizationDate.ValueOrDefault;

        // Получение логина пользователя.
        var getUserLoginResult = UserLogin.Create(_userDao.Login);
        if (getUserLoginResult.IsFailed)
        {
            errors.AddRange(getUserAuthorizationDate.Errors);
        }

        var login = getUserLoginResult.ValueOrDefault;

        // Получение пользователя VK.
        VkUser? vkUser = null;
        if (_userDao.VkUser is not null)
        {
            var getVkUserResult = RestoreVkUser(_userDao.VkUser);
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
    /// <param name="vkUserDao">Объект доступа данных пользователя VK.</param>
    /// <returns>Пользователь VK.</returns>
    private static Result<VkUser> CreateVkUser(VkUserDao vkUserDao)
    {
        var getVkInternalUserIdResult = VkInternalUserId.Create(vkUserDao.InternalUserId ?? string.Empty);
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
    /// <param name="vkUserDao">Объект доступа данных пользователя VK.</param>
    /// <returns>Пользователь VK.</returns>
    private static Result<VkUser> RestoreVkUser(VkUserDao vkUserDao)
    {
        var getVkInternalUserIdResult = VkInternalUserId.Create(vkUserDao.InternalUserId ?? string.Empty);
        if (getVkInternalUserIdResult.IsFailed)
        {
            return getVkInternalUserIdResult.ToResult();
        }

        var vkInternalUserId = getVkInternalUserIdResult.ValueOrDefault;

        var getVkUserResult = VkUser.Restore(VkUserId.Create(vkUserDao.Id), vkInternalUserId);
        if (getVkUserResult.IsFailed)
        {
            return getVkUserResult.ToResult();
        }

        return getVkUserResult.ValueOrDefault;
    }
}