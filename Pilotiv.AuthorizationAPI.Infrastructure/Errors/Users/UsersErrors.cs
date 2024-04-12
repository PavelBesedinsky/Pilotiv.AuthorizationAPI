using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Errors.Users;

public static class UsersErrors
{
    public static Error UserNotFoundById(UserId userId) =>
        new($"Не удалось найти пользователя по идентификатору ({userId.Value})");
    public static Error UserNotFoundByLogin(UserLogin userLogin) =>
        new($"Не удалось найти пользователя по логину ({userLogin.Value})");
    public static Error UserNotFoundByEmail(UserEmail userEmail) =>
        new($"Не удалось найти пользователя по адресу электронной почты ({userEmail.Value})");
    public static Error UserNotFoundByInternalId(VkInternalUserId internalId) =>
        new($"Не удалось найти пользователя по внутреннему идентификатору пользователя в VK ({internalId.Value})");
}