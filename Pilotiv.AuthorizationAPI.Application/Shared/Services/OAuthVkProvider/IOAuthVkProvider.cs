using FluentResults;
using Pilotiv.AuthorizationAPI.Application.Shared.Dtos;

namespace Pilotiv.AuthorizationAPI.Application.Shared.Services.OAuthVkProvider;

/// <summary>
/// Интерфейс взаимодействия с сервисом авторизации VK. 
/// </summary>
public interface IOAuthVkProvider
{
    /// <summary>
    /// Получение токена доступа от сервера авторизации VK.
    /// </summary>
    /// <param name="clientId">Идентификатор приложения.</param>
    /// <param name="clientSecret">Защищенный ключ приложения.</param>
    /// <param name="redirectUri">Доверенный Redirect URL.</param>
    /// <param name="code">Временный код, полученный после прохождения авторизации.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Модель ответа от сервера авторизации VK в формате JSON.</returns>
    public Task<Result<VkAccessTokenPayload>> GetAccessTokenAsync(string clientId, string clientSecret, string redirectUri, string code,
        CancellationToken cancellationToken = default);
}