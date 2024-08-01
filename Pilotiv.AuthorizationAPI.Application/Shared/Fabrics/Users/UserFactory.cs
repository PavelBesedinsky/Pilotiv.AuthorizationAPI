using FluentResults;
using Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users.Dtos;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Entities;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

namespace Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users;

/// <summary>
/// Фабрика пользователей.
/// </summary>
public class UserFactory : IEntityFactory<User, UserId>
{
    private readonly UsersFactoryUserPayload _payload;

    /// <summary>
    /// Создание фабрики пользователей.
    /// </summary>
    /// <param name="payload">Объект переноса данных информации о пользователе для фабрики пользователей.</param>
    public UserFactory(UsersFactoryUserPayload payload)
    {
        _payload = payload;
    }

    /// <inheritdoc />
    public Result<User> Create()
    {
        // Получение хэша-пароля пользователя.
        UserPassword? passwordHash = null;
        if (!string.IsNullOrWhiteSpace(_payload.PasswordHash))
        {
            var getPasswordHashResult = UserPassword.Create(_payload.PasswordHash, _payload.Salt ?? string.Empty);
            if (getPasswordHashResult.IsFailed)
            {
                return Result.Fail(getPasswordHashResult.Errors);
            }

            passwordHash = getPasswordHashResult.ValueOrDefault;
        }

        // Получение адреса электронной почты пользователя.
        var getUserEmailResult = UserEmail.Create(_payload.Email ?? string.Empty);
        if (getUserEmailResult.IsFailed)
        {
            return Result.Fail(getUserEmailResult.Errors);
        }

        var email = getUserEmailResult.ValueOrDefault;

        // Получение даты регистрации пользователя.
        var getUserRegistrationDate = UserRegistrationDate.Create(_payload.RegistrationDate);
        if (getUserRegistrationDate.IsFailed)
        {
            return Result.Fail(getUserRegistrationDate.Errors);
        }

        var registrationDate = getUserRegistrationDate.ValueOrDefault;

        // Получение даты авторизации пользователя.
        var getUserAuthorizationDateResult = UserAuthorizationDate.Create(_payload.AuthorizationDate);
        if (getUserAuthorizationDateResult.IsFailed)
        {
            return Result.Fail(getUserAuthorizationDateResult.Errors);
        }

        var authorizationDate = getUserAuthorizationDateResult.ValueOrDefault;

        UserLogin? login = null;
        if (!string.IsNullOrWhiteSpace(_payload.Login))
        {
            // Получение логина пользователя.
            var getUserLoginResult = UserLogin.Create(_payload.Login);
            if (getUserLoginResult.IsFailed)
            {
                return Result.Fail(getUserLoginResult.Errors);
            }

            login = getUserLoginResult.ValueOrDefault;
        }

        // Создание пользователя.
        var createUserResult = User.Create(passwordHash, email, registrationDate, authorizationDate, login);
        if (createUserResult.IsFailed)
        {
            return Result.Fail(createUserResult.Errors);
        }

        var user = createUserResult.ValueOrDefault;

        // Создание пользователя VK.
        if (_payload.VkUser is not null)
        {
            var vkUserFactory = new VkUserFactory(_payload.VkUser);

            var getVkUserResult = vkUserFactory.Create();
            if (getVkUserResult.IsFailed)
            {
                return Result.Fail(getVkUserResult.Errors);
            }

            var addVkUserResult = user.AddVkUser(getVkUserResult.ValueOrDefault);
            if (addVkUserResult.IsFailed)
            {
                return Result.Fail(addVkUserResult.Errors);
            }
        }

        // Создание токенов доступа.
        foreach (var refreshTokenPayload in _payload.RefreshTokens)
        {
            var refreshTokenFactory = new RefreshTokenFactory(refreshTokenPayload);

            var createRefreshTokenResult = refreshTokenFactory.Create();
            if (createRefreshTokenResult.IsFailed)
            {
                return Result.Fail(createRefreshTokenResult.Errors);
            }

            var addRefreshTokenResult = user.AddRefreshToken(createRefreshTokenResult.ValueOrDefault);
            if (addRefreshTokenResult.IsFailed)
            {
                return Result.Fail(addRefreshTokenResult.Errors);
            }
        }


        return user;
    }

    /// <inheritdoc />
    public Result<User> Restore()
    {
        // Получение хэша-пароля пользователя.
        UserPassword? passwordHash = null;
        if (!string.IsNullOrWhiteSpace(_payload.PasswordHash))
        {
            var getPasswordHashResult = UserPassword.Create(_payload.PasswordHash, _payload.Salt ?? string.Empty);
            if (getPasswordHashResult.IsFailed)
            {
                return Result.Fail(getPasswordHashResult.Errors);
            }

            passwordHash = getPasswordHashResult.ValueOrDefault;
        }

        // Получение адреса электронной почты пользователя.
        var getUserEmailResult = UserEmail.Create(_payload.Email ?? string.Empty);
        if (getUserEmailResult.IsFailed)
        {
            return Result.Fail(getUserEmailResult.Errors);
        }

        var email = getUserEmailResult.ValueOrDefault;

        // Получение даты регистрации пользователя.
        var getUserRegistrationDate = UserRegistrationDate.Create(_payload.RegistrationDate);
        if (getUserRegistrationDate.IsFailed)
        {
            return Result.Fail(getUserRegistrationDate.Errors);
        }

        var registrationDate = getUserRegistrationDate.ValueOrDefault;

        // Получение даты авторизации пользователя.
        var getUserAuthorizationDate = UserAuthorizationDate.Create(_payload.AuthorizationDate);
        if (getUserAuthorizationDate.IsFailed)
        {
            return Result.Fail(getUserAuthorizationDate.Errors);
        }

        var authorizationDate = getUserAuthorizationDate.ValueOrDefault;

        // Получение логина пользователя.
        var getUserLoginResult = UserLogin.Create(_payload.Login ?? string.Empty);
        if (getUserLoginResult.IsFailed)
        {
            return Result.Fail(getUserLoginResult.Errors);
        }

        var login = getUserLoginResult.ValueOrDefault;

        // Получение пользователя VK.
        VkUser? vkUser = null;
        if (_payload.VkUser is not null)
        {
            var vkUserFactory = new VkUserFactory(_payload.VkUser);

            var restoreVkUserResult = vkUserFactory.Restore();
            if (restoreVkUserResult.IsFailed)
            {
                return Result.Fail(restoreVkUserResult.Errors);
            }

            vkUser = restoreVkUserResult.ValueOrDefault;
        }

        // Получение токенов обновления.
        List<RefreshToken> refreshTokens = new();
        foreach (var refreshTokenPayload in _payload.RefreshTokens)
        {
            var refreshTokenFactory = new RefreshTokenFactory(refreshTokenPayload);

            var restoreRefreshTokenResult = refreshTokenFactory.Restore();
            if (restoreRefreshTokenResult.IsFailed)
            {
                return Result.Fail(restoreRefreshTokenResult.Errors);
            }

            refreshTokens.Add(restoreRefreshTokenResult.ValueOrDefault);
        }

        return User.Restore(UserId.Create(_payload.Id), passwordHash, email, registrationDate, authorizationDate, login,
            vkUser, refreshTokens);
    }
}