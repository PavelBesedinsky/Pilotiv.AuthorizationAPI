using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pilotiv.AuthorizationAPI.Application.Requests.Registration.Commands.Register;
using Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.ObtainVkToken;
using Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.ObtainVkToken.Dtos;
using Pilotiv.AuthorizationAPI.WebUI.Dtos.Register;

namespace Pilotiv.AuthorizationAPI.WebUI.Controllers;

/// <summary>
/// Контроллер авторизации.
/// </summary>
public class AuthController : ApiControllerBase
{
    /// <summary>
    /// Создание контроллера авторизации.
    /// </summary>
    /// <param name="mediator">Медиатор.</param>
    public AuthController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Получение токена.
    /// </summary>
    /// <param name="code">Код авторизации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpPost("token/vk")]
    public async Task<ActionResult<ObtainVkTokenCommandResponse>> ObtainVkTokenAsync([FromQuery] string code,
        CancellationToken cancellationToken = default)
    {
        var command = new ObtainVkTokenCommand(code);
        var commandResult = await Mediator.Send(command, cancellationToken);
        if (commandResult.IsFailed)
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        return commandResult.ValueOrDefault;
    }

    /// <summary>
    /// Получение токена.
    /// </summary>
    /// <param name="code">Код авторизации.</param>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ObtainVkTokenCommandResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("token/vk/mock")]
    [AllowAnonymous]
    public Task<ActionResult<ObtainVkTokenCommandResponse>> MockObtainVkTokenAsync([FromQuery] string code)
    {
        return Task.FromResult<ActionResult<ObtainVkTokenCommandResponse>>(new ObtainVkTokenCommandResponse
        {
            AccessToken =
                "vk1.a.FWoisVC_mm4NtnIo0fYgl7V-y4yv3xfSfbslzKkU_49EppWlZFEnujdokhE7hLxPRDBAMPeAVe0HJCt7FxPx7zMqq5Ty4rHcjwetdWpzhnA5URemexgiQeob62IWE-LRs-2l6K3wLs1Ktvldfs-HPAPzl3Udm6AGx2veR-CgoInS1cdKwaNb_N4Zs5FE0XEoP94pBYnJbpIlmdSB8LXdOQ",
            IsNew = true
        });
    }

    /// <summary>
    /// Регистрация пользователя.
    /// </summary>
    /// <param name="request">Объект переноса данных запроса команды регистрации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> RegisterAsync([FromBody] RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Login))
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest();
        }

        var command = new RegisterCommand(request.Login, request.Password, request.Email);
        var commandResult = await Mediator.Send(command, cancellationToken);
        if (commandResult.IsFailed)
        {
            return GetStatusCodeForError(commandResult.Errors);
        }

        return Ok();
    }

    private ActionResult GetStatusCodeForError(IEnumerable<IError> errors)
    {
        return StatusCode(StatusCodes.Status409Conflict);
    }
}