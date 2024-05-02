using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.ObtainVkToken.Errors;

public static class ObtainVkTokenErrors
{
    public static Error CodeIsNullOrEmpty() => new("Не указан временный код.");
    public static Error ClientIdIsNullOrEmpty() => new("Не указан идентификатор приложения.");
    public static Error ClientSecretIsNullOrEmpty() => new("Не указан защищенный ключ приложения.");
    public static Error RedirectUriIsNullOrEmpty() => new("Не указан адрес переадресации.");
    public static Error EmailIsOccupied(UserEmail email) =>
        new($"Адрес электронной почты ({email.Value}) уже используется.");
}