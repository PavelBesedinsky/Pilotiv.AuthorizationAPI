namespace Pilotiv.AuthorizationAPI.Domain.Primitives;

/// <summary>
/// Примитив идентификатора агрегата.
/// </summary>
/// <typeparam name="TValue">Тип идентификатора.</typeparam>
public abstract class AggregateRootId<TValue> : ValueObject
{
    /// <summary>
    /// Значение идентификатора.
    /// </summary>
    public abstract TValue Value { get; }
}