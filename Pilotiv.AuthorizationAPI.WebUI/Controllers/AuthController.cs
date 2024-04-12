using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.ObtainVkToken;

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
    public async Task<ActionResult> ObtainVkTokenAsync([FromQuery] string code,
        CancellationToken cancellationToken = default)
    {
        var command = new ObtainVkTokenCommand(code);
        var commandResult = await Mediator.Send(command, cancellationToken);
        if (commandResult.IsFailed)
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        return Ok();
    }
}