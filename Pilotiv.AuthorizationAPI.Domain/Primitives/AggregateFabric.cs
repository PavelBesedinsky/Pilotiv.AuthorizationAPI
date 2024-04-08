using FluentResults;

namespace Pilotiv.AuthorizationAPI.Domain.Primitives;

/// <summary>
/// Фабрика агрегатов.
/// </summary>
public abstract class AggregateFabric<TAggregate, TId, TIdType>
    where TAggregate : AggregateRoot<TId, TIdType>
    where TId : AggregateRootId<TIdType>
{
    /// <summary>
    /// Создание нового агрегата.
    /// </summary>
    public abstract Result<TAggregate> Create();

    /// <summary>
    /// Востановление агрегата.
    /// </summary>
    /// <param name="id">Идентификатор модели.</param>
    public abstract Result<TAggregate> Restore(TIdType id);
}