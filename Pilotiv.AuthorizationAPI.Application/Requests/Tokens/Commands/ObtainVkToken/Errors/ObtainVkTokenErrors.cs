namespace Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.ObtainVkToken.Errors;

public static class ObtainVkTokenErrors
{
    public static string CodeIsNullOrEmpty() => "Не указан временный код.";
    public static string ClientIdIsNullOrEmpty() => "Не указан идентификатор приложения.";
    public static string ClientSecretIsNullOrEmpty() => "Не указан защищенный ключ приложения.";
    public static string RedirectUriIsNullOrEmpty() => "Не указан адрес переадресации.";

}