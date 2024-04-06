using FluentResults;
using MediatR;
using Microsoft.Extensions.Options;
using Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.ObtainVkToken.Dtos;
using Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.ObtainVkToken.Errors;
using Pilotiv.AuthorizationAPI.Application.Shared.Options;
using Pilotiv.AuthorizationAPI.Application.Shared.Services;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.ObtainVkToken;

/// <summary>
/// Обработчик команды получения токена VK.
/// </summary>
public class ObtainVkTokenCommandHandler : IRequestHandler<ObtainVkTokenCommand, Result<ObtainVkTokenCommandResponse>>
{
    private readonly IOAuthVkProvider _oAuthVkProvider;
    private readonly OAuthVkCredentialsOptions _oAuthVkCredentials;

    /// <summary>
    /// Создание обработчика команды получения токена VK.
    /// </summary>
    /// <param name="oAuthVkCredentialsOptionsMonitor">Настройки для Authorization Code Flow для получения Access token пользователя.</param>
    /// <param name="oAuthVkProvider">Провайдер взаимодействия с сервисом авторизации VK.</param>
    public ObtainVkTokenCommandHandler(IOptionsMonitor<OAuthVkCredentialsOptions> oAuthVkCredentialsOptionsMonitor,
        IOAuthVkProvider oAuthVkProvider)
    {
        _oAuthVkProvider = oAuthVkProvider;
        _oAuthVkCredentials = oAuthVkCredentialsOptionsMonitor.CurrentValue;
    }

    /// <summary>
    /// Обработка команды получения токена VK.
    /// </summary>
    /// <param name="request">Команда получения токена VK.</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект переноса данных результата команды получения токена VK.</returns>
    public async Task<Result<ObtainVkTokenCommandResponse>> Handle(ObtainVkTokenCommand request,
        CancellationToken cancellationToken = default)
    {
        var code = request.Code;
        if (string.IsNullOrWhiteSpace(code))
        {
            return Result.Fail(ObtainVkTokenErrors.CodeIsNullOrEmpty());
        }

        if (string.IsNullOrWhiteSpace(_oAuthVkCredentials.ClientId))
        {
            return Result.Fail(ObtainVkTokenErrors.ClientIdIsNullOrEmpty());
        }

        if (string.IsNullOrWhiteSpace(_oAuthVkCredentials.ClientSecret))
        {
            return Result.Fail(ObtainVkTokenErrors.ClientSecretIsNullOrEmpty());
        }

        if (string.IsNullOrWhiteSpace(_oAuthVkCredentials.RedirectUri))
        {
            return Result.Fail(ObtainVkTokenErrors.RedirectUriIsNullOrEmpty());
        }

        var getAccessTokenResult = await _oAuthVkProvider.GetAccessTokenAsync(_oAuthVkCredentials.ClientId,
            _oAuthVkCredentials.ClientSecret, _oAuthVkCredentials.RedirectUri, code, cancellationToken);
        if (getAccessTokenResult.IsFailed)
        {
            return Result.Fail(getAccessTokenResult.Errors);
        }

        return new ObtainVkTokenCommandResponse
        {
            AccessToken = getAccessTokenResult.Value.Access_Token,
            IsNew = true
        };
    }
}