using FluentResults;
using MediatR;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.RevokeRefreshToken;

/// <summary>
/// Команда отзыва токена обновления.
/// </summary>
public class RevokeRefreshTokenCommand : IRequest<Result>
{
    /// <summary>
    /// Токен обновления.
    /// </summary>
    public string RefreshToken { get; }
    
    /// <summary>
    /// Причина отзыва токена.
    /// </summary>
    public string? Reason { get; init; }

    /// <summary>
    /// IP-адрес, с которого выполняется отзыв токена обновления.
    /// </summary>
    public string? Ip { get; init; }

    /// <summary>
    /// Создание команды отзыва токена обновления.
    /// </summary>
    /// <param name="refreshToken">Токен обновления.</param>
    public RevokeRefreshTokenCommand(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}