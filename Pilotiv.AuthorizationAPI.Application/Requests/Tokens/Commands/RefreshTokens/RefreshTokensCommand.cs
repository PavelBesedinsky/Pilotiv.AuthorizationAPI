using FluentResults;
using MediatR;
using Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.RefreshTokens.Dtos;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.RefreshTokens;

/// <summary>
/// Команда обновления токенов.
/// </summary>
public class RefreshTokensCommand : IRequest<Result<RefreshTokensCommandResponse>>
{
    /// <summary>
    /// Токен обновления.
    /// </summary>
    public string RefreshToken { get; }

    /// <summary>
    /// IP-адрес, с которого выполняется запрос обновления токенов.
    /// </summary>
    public string? Ip { get; init; }
    
    /// <summary>
    /// Создание команды обновления токенов.
    /// </summary>
    /// <param name="refreshToken">Токен обновления.</param>
    public RefreshTokensCommand(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}