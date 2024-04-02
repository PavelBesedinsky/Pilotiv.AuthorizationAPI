using FluentResults;
using MediatR;
using Microsoft.Extensions.Options;
using Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.ObtainVkToken.Dtos;
using Pilotiv.AuthorizationAPI.Application.Shared.Options;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.ObtainVkToken;

/// <summary>
/// Обработчик команды получения токена VK.
/// </summary>
public class ObtainVkTokenCommandHandler : IRequestHandler<ObtainVkTokenCommand, Result<ObtainVkTokenCommandResponse>>
{
    private readonly OAuthVkCredentialsOptions _oAuthVkCredentials;

    /// <summary>
    /// Создание обработчика команды получения токена VK.
    /// </summary>
    /// <param name="oAuthVkCredentialsOptionsMonitor">Настройки для Authorization Code Flow для получения Access token пользователя.</param>
    public ObtainVkTokenCommandHandler(IOptionsMonitor<OAuthVkCredentialsOptions> oAuthVkCredentialsOptionsMonitor)
    {
        _oAuthVkCredentials = oAuthVkCredentialsOptionsMonitor.CurrentValue;
    }
    
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