﻿using FluentResults;
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
        UserAuthorizationDate authorizationDate, UserLogin? login, VkUser? vkUser) : base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
        RegistrationDate = registrationDate;
        AuthorizationDate = authorizationDate;
        Login = login;
        VkUser = vkUser;
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
        UserAuthorizationDate authorizationDate, UserLogin? login, VkUser? vkUser) : this(UserId.Create(), passwordHash,
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
    /// <returns>Пользователь.</returns>
    public static Result<User> Create(UserPasswordHash? userPasswordHash, UserEmail email,
        UserRegistrationDate registrationDate, UserAuthorizationDate authorizationDate, UserLogin? login)
    {
        var user = new User(userPasswordHash, email, registrationDate, authorizationDate, login, null);

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
    /// <returns>Пользователь.</returns>
    public static Result<User> Restore(UserId userId, UserPasswordHash? userPasswordHash, UserEmail email,
        UserRegistrationDate registrationDate, UserAuthorizationDate authorizationDate, UserLogin login, VkUser? vkUser)
    {
        return new User(userId, userPasswordHash, email, registrationDate, authorizationDate, login, vkUser);
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
}