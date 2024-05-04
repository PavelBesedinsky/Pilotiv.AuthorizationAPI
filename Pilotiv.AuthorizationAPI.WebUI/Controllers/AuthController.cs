using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.Authorize;
using Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.Authorize.Dtos;
using Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.ObtainVkToken;
using Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.ObtainVkToken.Dtos;
using Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.Register;
using Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.RevokeRefreshToken;
using Pilotiv.AuthorizationAPI.WebUI.Dtos.AuthController;

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
    /// <param name="request">Объект переноса данных команды регистрации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [AllowAnonymous]
    [HttpPost("register")]
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
            return StatusCode(StatusCodes.Status409Conflict);
        }

        return Ok();
    }

    /// <summary>
    /// Авторизация пользователя.
    /// </summary>
    /// <param name="request">Объект переноса данных команды авторизации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объекта переноса данных ответа команды авторизации.</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorizeCommandResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AllowAnonymous]
    [HttpPost("authorize")]
    public async Task<ActionResult<AuthorizeCommandResponse>> AuthorizeAsync([FromBody] AuthorizeRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Login))
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        var ip = GetIpAddress();
        if (string.IsNullOrWhiteSpace(ip))
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        var authorizeCommand = new AuthorizeCommand(request.Login, request.Password, ip);
        var authorizeResult = await Mediator.Send(authorizeCommand, cancellationToken);
        if (authorizeResult.IsFailed)
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        var resultValue = authorizeResult.ValueOrDefault;

        var refreshToken = resultValue.RefreshToken;
        if (refreshToken is not null)
        {
            AddRefreshTokenToHttpOnly(refreshToken.Token ?? string.Empty, refreshToken.Expires);
        }

        return resultValue;
    }

    /// <summary>
    /// Отзыв токена обновления.
    /// </summary>
    /// <param name="request">Объект переноса данных запроса отзыва токена обновления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorizeCommandResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    [HttpPost("revoke_refresh_token")]
    public async Task<ActionResult> RevokeRefreshTokenAsync(
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        RevokeRefreshTokenRequest? request = default,
        CancellationToken cancellationToken = default)
    {
        var refreshToken = string.IsNullOrWhiteSpace(request?.RefreshToken)
            ? Request.Cookies["refreshToken"]
            : request.RefreshToken;
        var reason = request?.Reason ?? string.Empty;

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return BadRequest("Неверный параметр запроса.");
        }

        var command = new RevokeRefreshTokenCommand(refreshToken)
        {
            Reason = reason,
            Ip = GetIpAddress()
        };
        
        var commandResult = await Mediator.Send(command, cancellationToken);
        if (commandResult.IsFailed)
        {
            return BadRequest("Ошибка при выполнении команды отзыва токена.");
        }

        return Ok();
    }

    /// <summary>
    /// Получение IP адреса
    /// </summary>
    /// <returns>IP адрес</returns>
    private string? GetIpAddress()
    {
        if (Request.Headers.TryGetValue("X-Forwarded-For", out var ipAddress))
        {
            return ipAddress.FirstOrDefault();
        }

        return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
    }

    /// <summary>
    /// Добавление Refresh Token в httpOnly Cookie
    /// </summary>
    /// <param name="token">Токен.</param>
    /// <param name="expires">Дата истечения срока жизни токен аобновления.</param>
    private void AddRefreshTokenToHttpOnly(string token, DateTime expires)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return;
        }

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires
        };

        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }
}