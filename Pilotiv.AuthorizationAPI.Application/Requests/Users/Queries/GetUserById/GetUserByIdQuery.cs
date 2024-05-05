using FluentResults;
using MediatR;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Users.Queries.GetUserById;

/// <summary>
/// Команда получения пользователя по идентификатору.
/// </summary>
public class GetUserByIdQuery : IRequest<Result<User>>
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Создание команды получения пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    public GetUserByIdQuery(Guid id)
    {
        Id = id;
    }
}