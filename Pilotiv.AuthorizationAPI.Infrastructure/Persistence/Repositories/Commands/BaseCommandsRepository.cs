using System.Data;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Repositories.Commands;

/// <summary>
/// Базовый репозиторий команд.
/// </summary>
public abstract class BaseCommandsRepository
{
    /// <summary>
    /// Вызов обработчика.
    /// </summary>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    /// <param name="entityWithDomainEvents">Доменная сущность.</param>
    /// <param name="domainEventsHandler">Обработчик доменных событий.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    protected static async Task InvokeAsync(IDbConnection connection, IDbTransaction transaction, IHasDomainEvents entityWithDomainEvents,
        Func<IDbConnection, IDbTransaction, IDomainEvent, Task> domainEventsHandler,
        CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in entityWithDomainEvents.DomainEvents)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await domainEventsHandler(connection, transaction, domainEvent);
        }

        entityWithDomainEvents.ClearDomainEvents();
    }
}