using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pilotiv.AuthorizationAPI.Application.Requests.Users.Queries.GetUserById;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;

namespace Pilotiv.AuthorizationAPI.WebUI.Controllers;

/// <summary>
/// Контроллер пользователей.
/// </summary>
public class UsersController : ApiControllerBase
{
    /// <summary>
    /// Создание контроллера пользователей.
    /// </summary>
    /// <param name="mediator">Медиатор.</param>
    public UsersController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Получение пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Пользователь.</returns>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserByIdAsync([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUserByIdQuery(id);
        var queryResult = await Mediator.Send(query, cancellationToken);
        if (queryResult.IsFailed)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, queryResult.ToString());
        }

        return queryResult.ValueOrDefault;
    }
}