using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    [HttpPost("token")]
    public async Task ObtainTokenAsync()
    {
        return;
    }
}