namespace Pilotiv.AuthorizationAPI.Domain.Primitives;

/// <summary>
/// Интерфейс, определяющий наличие доменных событий.
/// </summary>
public interface IHasDomainEvents
{
    /// <summary>
    /// Доменные события.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Очистить доменные события.
    /// </summary>
    public void ClearDomainEvents();
}