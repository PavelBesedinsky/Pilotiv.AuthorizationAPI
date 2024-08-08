using FluentResults;
using MediatR;
using Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.ObtainVkToken.Dtos;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.ObtainVkToken;

/// <summary>
/// Команда получения токена VK.
/// </summary>
public class ObtainVkTokenCommand : IRequest<Result<ObtainVkTokenCommandResponse>>
{
    /// <summary>
    /// Код авторизации.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Создание команды получения токена VK.
    /// </summary>
    /// <param name="code">Код авторизации.</param>
    public ObtainVkTokenCommand(string code)
    {
        Code = code;
    }
}