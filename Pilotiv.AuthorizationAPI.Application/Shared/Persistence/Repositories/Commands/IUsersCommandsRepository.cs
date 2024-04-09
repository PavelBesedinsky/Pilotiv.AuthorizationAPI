using Pilotiv.AuthorizationAPI.Domain.Models.Users;

namespace Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Commands;

/// <summary>
/// Интерфейс репозитория команд пользователя.
/// </summary>
public interface IUsersCommandsRepository : IUnitOfWork<User>
{
}