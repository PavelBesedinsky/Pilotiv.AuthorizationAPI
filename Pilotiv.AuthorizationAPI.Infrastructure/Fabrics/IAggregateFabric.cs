using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Fabrics;

/// <summary>
/// Фабрика агрегатов.
/// </summary>
public interface IAggregateFabric<TAggregate, TId, in TIdType>
    where TAggregate : AggregateRoot<TId, TIdType>
    where TId : AggregateRootId<TIdType>
{
    /// <summary>
    /// Создание нового агрегата.
    /// </summary>
    public Result<TAggregate> Create();

    /// <summary>
    /// Востановление агрегата.
    /// </summary>
    /// <param name="id">Идентификатор модели.</param>
    public Result<TAggregate> Restore(TIdType id);
}