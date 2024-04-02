using FluentResults;
using MediatR;
using Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.ObtainVkToken.Dtos;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.ObtainVkToken;

/// <summary>
/// Обработчик команды получения токена VK.
/// </summary>
public class ObtainVkTokenCommandHandler : IRequestHandler<ObtainVkTokenCommand, Result<ObtainVkTokenCommandResponse>>
{
    /// <summary>
    /// Обработка команды получения токена VK.
    /// </summary>
    /// <param name="request">Команда получения токена VK.</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект переноса данных результата команды получения токена VK.</returns>
    public async Task<Result<ObtainVkTokenCommandResponse>> Handle(ObtainVkTokenCommand request, CancellationToken cancellationToken = default)
    {
        return new ObtainVkTokenCommandResponse();
    }
}