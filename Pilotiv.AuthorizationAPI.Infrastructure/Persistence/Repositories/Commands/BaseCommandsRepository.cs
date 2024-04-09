using System.Data;
using Pilotiv.AuthorizationAPI.Domain.Primitives;
using Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Context;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Repositories.Commands;

/// <summary>
/// Базовый репозиторий команд.
/// </summary>
public abstract class BaseCommandsRepository
{
    /// <summary>
    /// Вызов обработчика.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    /// <param name="entityWithDomainEvents">Доменная сущность.</param>
    /// <param name="domainEventsHandler">Обработчик доменных событий.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    protected static async Task InvokeAsync(DbContext dbContext, IHasDomainEvents entityWithDomainEvents,
        Func<IDbConnection, IDbTransaction, IDomainEvent, Task> domainEventsHandler,
        CancellationToken cancellationToken = default)
    {
        using var connection = dbContext.CreateOpenedConnection();
        var trx = connection.BeginTransaction();

        foreach (var domainEvent in entityWithDomainEvents.DomainEvents)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            await domainEventsHandler(connection, trx, domainEvent);
        }

        trx.Commit();
        entityWithDomainEvents.ClearDomainEvents();
    }
}