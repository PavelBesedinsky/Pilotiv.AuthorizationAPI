using FluentResults;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.ObtainVkToken.Errors;

public static class ObtainVkTokenErrors
{
    public static Error CodeIsNullOrEmpty() => new("Не указан временный код.");
    public static Error ClientIdIsNullOrEmpty() => new("Не указан идентификатор приложения.");
    public static Error ClientSecretIsNullOrEmpty() => new("Не указан защищенный ключ приложения.");
    public static Error RedirectUriIsNullOrEmpty() => new("Не указан адрес переадресации.");

}