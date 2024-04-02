using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Pilotiv.AuthorizationAPI.WebUI.Controllers;

/// <summary>
/// Базовый контроллер.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>
    /// Медиатор.
    /// </summary>
    protected IMediator Mediator { get; }

    /// <summary>
    /// Создание базового контроллера.
    /// </summary>
    /// <param name="mediator">Медиатор</param>
    protected ApiControllerBase(IMediator mediator)
    {
        Mediator = mediator;
    }
}