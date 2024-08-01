using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using FluentResults;
using Microsoft.Extensions.Logging;
using Pilotiv.AuthorizationAPI.Application.Shared.Dtos;
using Pilotiv.AuthorizationAPI.Application.Shared.Services;
using Pilotiv.AuthorizationAPI.Infrastructure.Services.OAuth.Errors;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Services.OAuth;

/// <summary>
/// Провайдер взаимодействия с сервисом авторизации VK.
/// </summary>
public class OAuthVkProvider : IOAuthVkProvider, IDisposable
{
    private readonly ILogger<OAuthVkProvider> _logger;
    private readonly HttpClientHandler _handler;
    private readonly HttpClient _httpClint;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    /// <summary>
    /// Создание провайдера взаимодейтсвия с сервисом авторизации VK.
    /// </summary>
    public OAuthVkProvider(ILogger<OAuthVkProvider> logger)
    {
        _logger = logger;
        _handler = new HttpClientHandler();
        _handler.ServerCertificateCustomValidationCallback += OnServerCertificateCustomValidationCallback;

        _httpClint = new HttpClient(_handler);
        _httpClint.Timeout = TimeSpan.FromSeconds(20);

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    /// <summary>
    /// Освобождение ресурсов.
    /// </summary>
    public void Dispose()
    {
        _handler.ServerCertificateCustomValidationCallback -= OnServerCertificateCustomValidationCallback;
    }

    /// <summary>
    /// Обработка валидации пользовательского сертификата.
    /// </summary>
    private bool OnServerCertificateCustomValidationCallback(HttpRequestMessage httpRequestMessage,
        X509Certificate2? x509Certificate2,
        X509Chain? x509Chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    /// <inheritdoc />
    public async Task<Result<VkAccessTokenPayload>> GetAccessTokenAsync(string clientId, string clientSecret,
        string redirectUri, string code, CancellationToken cancellationToken)
    {
        var response = await _httpClint.GetAsync(
            $"https://oauth.vk.com/access_token?client_id={clientId}&client_secret={clientSecret}&redirect_uri={redirectUri}&code={code}",
            cancellationToken);
        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var deserializedErrorModel =
                await JsonSerializer.DeserializeAsync<VkAccessTokenError>(responseStream, _jsonSerializerOptions,
                    cancellationToken);
            if (deserializedErrorModel is null)
            {
                return OAuthVkProviderErrors.FailedToDeserializeError();
            }

            return OAuthVkProviderErrors.FailedToGetAccessToken(deserializedErrorModel.Error_Description ??
                                                                "empty error description");
        }

        var deserializedModel =
            await JsonSerializer.DeserializeAsync<VkAccessTokenPayload>(responseStream, _jsonSerializerOptions,
                cancellationToken);
        if (deserializedModel is null)
        {
            return OAuthVkProviderErrors.FailedToDeserializeResponse();
        }

        return deserializedModel;
    }
}