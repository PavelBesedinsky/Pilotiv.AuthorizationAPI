namespace Pilotiv.AuthorizationAPI.Domain.Primitives;

/// <summary>
/// Примитив агрегата.
/// </summary>
public abstract class AggregateRoot<TId, TIdType> : Entity<TId> where TId : AggregateRootId<TIdType>
{
    /// <summary>
    /// Примитив агрегата.
    /// </summary>
    /// <param name="id">Идентификатор агрената</param>
    protected AggregateRoot(TId id) : base(id)
    {
    }
}