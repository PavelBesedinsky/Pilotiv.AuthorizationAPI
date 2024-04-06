namespace Pilotiv.AuthorizationAPI.Infrastructure.Services.Errors;

public static class OAuthVkProviderErrors
{
    private static string DefaultPrefix => "Произошла ошибка при получении токена доступа VK";
    public static string FailedToGetAccessToken(string description) => $"{DefaultPrefix}. Не удалось получить токен доступа VK: {description}";
    
    public static string FailedToDeserializeError() => $"{DefaultPrefix}. Не удалось десериализовать ошибку.";
    public static string FailedToDeserializeResponse() => $"{DefaultPrefix}. Не удалось десериализовать модель с токеном доступа.";


}