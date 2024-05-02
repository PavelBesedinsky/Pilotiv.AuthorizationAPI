using System.Text.Json.Serialization;
using Pilotiv.AuthorizationAPI.Application.Shared.Dtos;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.Authorize.Dtos;

/// <summary>
/// Объекта переноса данных ответа команды авторизации.
/// </summary>
public class AuthorizeCommandResponse
{
    /// <summary>
    /// Токен доступа.
    /// </summary>
    public string? AccessToken { get; init; }

    /// <summary>
    /// Токен обновления.
    /// </summary>
    [JsonIgnore]
    public RefreshTokenPayload? RefreshToken { get; init; }
}