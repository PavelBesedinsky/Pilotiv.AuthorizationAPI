using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Errors;

public static class UsersErrors
{
    public static Error UserPasswordHashIsNullOrEmpty => new("Не задан хэш-пароля пользователя");
    public static Error UserEmailIsNullOrEmpty() => new("Электронный адрес почты пользователя не задан.");
    public static Error InvalidUserEmail() => new("Электронный адрес почты не удолетворяет параметрам.");
    public static Error UserLoginIsNullOrEmpty() => new("Логин пользователя не задан.");

    public static Error InvalidUserLoginLength(int minLength, int maxLength) =>
        new($"Логин пользователя должен быть длинной от {minLength} до {maxLength} символов.");

    public static Error RegistrationDateIsNotSpecified() => new("Дата регистрации не указана");

    public static Error InvalidRegistrationDatePeriod(DateTime minDate) =>
        new($"Дата регистрации не должна быть меньше, чем {minDate}");

    public static Error AuthorizationDateIsNotSpecified() => new("Дата авторизации не указана");
    public static Error VkInternalUserIdIsEmpty() => new("Внутренний идентификатор пользователя VK пустой");
    public static Error ForbiddenToOverrideVkUser() => new("Запрещено переопределять пользователя VK.");
    public static Error InvalidAuthorizationDate() => new("Неверная дата авторизации пользователя.");

    public static Error RevokingRefreshTokenNotFound(UserId userId, RefreshTokenId refreshTokenId) =>
        new($"Для пользователя ({userId.Value}) не удалось найти отзываемый токен ({refreshTokenId.Value})");
}