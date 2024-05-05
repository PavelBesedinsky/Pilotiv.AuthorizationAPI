using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Entities;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Errors;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users;

/// <summary>
/// Пользователь.
/// </summary>
public class User : AggregateRoot<UserId, Guid>
{
    private readonly HashSet<RefreshToken> _refreshTokens;

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
    public UserLogin? Login { get; private set; }

    /// <summary>
    /// Дата регистрации пользователя.
    /// </summary>
    public UserRegistrationDate RegistrationDate { get; }

    /// <summary>
    /// Дата авторизации пользователя.
    /// </summary>
    public UserAuthorizationDate AuthorizationDate { get; private set; }

    /// <summary>
    /// Пользователь VK.
    /// </summary>
    public VkUser? VkUser { get; private set; }

    /// <summary>
    /// Токены обновления.
    /// </summary>
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;

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
    /// <param name="refreshTokens">Токены обновления.</param>
    private User(UserId id, UserPasswordHash? passwordHash, UserEmail email, UserRegistrationDate registrationDate,
        UserAuthorizationDate authorizationDate, UserLogin? login, VkUser? vkUser,
        IEnumerable<RefreshToken> refreshTokens) : base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
        RegistrationDate = registrationDate;
        AuthorizationDate = authorizationDate;
        Login = login;
        VkUser = vkUser;
        _refreshTokens = refreshTokens.ToHashSet();
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
    /// <param name="refreshTokens">Токены обновления.</param>
    private User(UserPasswordHash? passwordHash, UserEmail email, UserRegistrationDate registrationDate,
        UserAuthorizationDate authorizationDate, UserLogin? login, VkUser? vkUser,
        IEnumerable<RefreshToken> refreshTokens) : this(UserId.Create(), passwordHash,
        email, registrationDate, authorizationDate, login, vkUser, refreshTokens)
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
    /// <returns>Пользователь.</returns>
    public static Result<User> Create(UserPasswordHash? userPasswordHash, UserEmail email,
        UserRegistrationDate registrationDate, UserAuthorizationDate authorizationDate, UserLogin? login)
    {
        var user = new User(userPasswordHash, email, registrationDate, authorizationDate, login, null,
            Enumerable.Empty<RefreshToken>());

        user.AddDomainEvent(new UserCreatedDomainEvent(user.Id));
        if (userPasswordHash is not null)
        {
            user.AddDomainEvent(new UserPasswordHashChangedDomainEvent(user.Id, userPasswordHash));
        }

        user.AddDomainEvent(new UserEmailChangedDomainEvent(user.Id, email));
        user.AddDomainEvent(new UserRegistrationDateChangedDomainEvent(user.Id, registrationDate));
        user.AddDomainEvent(new UserAuthorizationDateChangedDomainEvent(user.Id, authorizationDate));
        if (login is not null)
        {
            user.AddDomainEvent(new UserLoginChangedDomainEvent(user.Id, login));
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
    /// <param name="refreshTokens">Токены обновления.</param>
    /// <returns>Пользователь.</returns>
    public static Result<User> Restore(UserId userId, UserPasswordHash? userPasswordHash, UserEmail email,
        UserRegistrationDate registrationDate, UserAuthorizationDate authorizationDate, UserLogin login, VkUser? vkUser,
        IEnumerable<RefreshToken> refreshTokens)
    {
        return new User(userId, userPasswordHash, email, registrationDate, authorizationDate, login, vkUser,
            refreshTokens);
    }

    /// <summary>
    /// Добавление пользователя VK.
    /// </summary>
    /// <param name="vkUser">Пользователь VK.</param>
    public Result AddVkUser(VkUser vkUser)
    {
        if (VkUser is not null)
        {
            return UsersErrors.ForbiddenToOverrideVkUser();
        }

        VkUser = vkUser;
        AddDomainEvent(new UserVkUserChangedDomainEvent(Id, vkUser));

        return Result.Ok();
    }

    /// <summary>
    /// Добавление токена обновления.
    /// </summary>
    /// <param name="refreshToken">Токен обновления.</param>
    public Result AddRefreshToken(RefreshToken refreshToken)
    {
        if (_refreshTokens.Any(token => token.Id == refreshToken.Id))
        {
            return Result.Ok();
        }

        _refreshTokens.Add(refreshToken);
        AddDomainEvent(new UserRefreshTokenAddedDomainEvent(Id, refreshToken));

        return Result.Ok();
    }

    /// <summary>
    /// Установка логина пользователя.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    public Result SetLogin(UserLogin login)
    {
        Login = login;
        AddDomainEvent(new UserLoginChangedDomainEvent(Id, login));

        return Result.Ok();
    }

    /// <summary>
    /// Обновление даты авторизации пользователя.
    /// </summary>
    /// <param name="authorizationDate">Дата авторизации пользователя.</param>
    /// <param name="force">Принудительное изменение даты. Обход запрета, что новая дата авторизации должна быть больше старой.</param>
    public Result UpdateAuthorizationDate(UserAuthorizationDate authorizationDate, bool force = false)
    {
        if (!force && authorizationDate.Value < AuthorizationDate.Value)
        {
            return UsersErrors.InvalidAuthorizationDate();
        }

        AuthorizationDate = authorizationDate;
        AddDomainEvent(new UserAuthorizationDateChangedDomainEvent(Id, authorizationDate));

        return Result.Ok();
    }

    /// <summary>
    /// Отзыв токена.
    /// </summary>
    /// <param name="revokingToken">Отзываемый токен.</param>
    /// <param name="ip">IP-адрес пользователя, запрашивающего отзыв токена.</param>
    /// <param name="reason">Причина отзыва токена.</param>
    /// <param name="replacingToken">Отзывающий токен.</param>
    public Result RevokeToken(RefreshToken revokingToken, string ip, string reason, RefreshToken? replacingToken = null)
    {
        if (!_refreshTokens.Contains(revokingToken))
        {
            return UsersErrors.RevokingRefreshTokenNotFound(Id, revokingToken.Id);
        }

        var revokeTokenResult = revokingToken.RevokeToken(ip, reason, replacingToken);
        if (revokeTokenResult.IsFailed)
        {
            return revokeTokenResult;
        }

        if (replacingToken is not null)
        {
            var addRefreshTokenResult = AddRefreshToken(replacingToken);
            if (addRefreshTokenResult.IsFailed)
            {
                return addRefreshTokenResult;
            }

            AddDomainEvent(new UserRefreshTokenRevokedDomainEvent(Id, revokingToken, replacingToken));
        }

        return Result.Ok();
    }
}