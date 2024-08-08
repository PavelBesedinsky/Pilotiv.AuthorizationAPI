using System.Text.Json.Serialization;
using Pilotiv.AuthorizationAPI.Application.Shared.Dtos;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.RefreshTokens.Dtos;

/// <summary>
/// Объекта переноса данных ответа команды обновления токенов.
/// </summary>
public class RefreshTokensCommandResponse
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