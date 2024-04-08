using FluentResults;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Services.Errors;

public static class OAuthVkProviderErrors
{
    private static string DefaultPrefix => "Произошла ошибка при получении токена доступа VK";

    public static Error FailedToGetAccessToken(string description) =>
        new($"{DefaultPrefix}. Не удалось получить токен доступа VK: {description}");

    public static Error FailedToDeserializeError() => new($"{DefaultPrefix}. Не удалось десериализовать ошибку.");

    public static Error FailedToDeserializeResponse() =>
        new($"{DefaultPrefix}. Не удалось десериализовать модель с токеном доступа.");
}