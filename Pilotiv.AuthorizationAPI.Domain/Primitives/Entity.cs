namespace Pilotiv.AuthorizationAPI.Domain.Primitives;

/// <summary>
/// Примитив сущности.
/// </summary>
public abstract class Entity<TId> : IHasDomainEvents, IEquatable<Entity<TId>>
    where TId : ValueObject
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Идентификатор сущности.
    /// </summary>
    public TId Id { get; protected init; }
    
    /// <inheritdoc cref="IHasDomainEvents.DomainEvents"/>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Добавление доменного события.
    /// </summary>
    /// <param name="domainEvent">Доменное событие.</param>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    
    /// <inheritdoc cref="IHasDomainEvents.ClearDomainEvents"/>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    
    /// <summary>
    /// Создание примитива сущности.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    protected Entity(TId id)
    {
        Id = id;
    }

    /// <summary>
    /// Получение признака равенства объектов.
    /// </summary>
    /// <param name="obj">Сравниваемый объект.</param>
    /// <returns>Признак равенства объектов</returns>
    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Id.Equals(entity.Id);
    }

    /// <summary>
    /// Получение признака равенства объектов.
    /// </summary>
    /// <param name="other">Сравниваемая сущность.</param>
    /// <returns>Признак равенства объектов</returns>
    public bool Equals(Entity<TId>? other)
    {
        return Equals((object?) other);
    }

    /// <summary>
    /// Перегрузка оператора "==".
    /// </summary>
    /// <param name="left">Значение слева от оператора.</param>
    /// <param name="right">Значение справа от оператора.</param>
    /// <returns>Признак раветства объектов.</returns>
    public static bool operator ==(Entity<TId> left, Entity<TId> right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Перегрузка оператора "!=".
    /// </summary>
    /// <param name="left">Значение слева от оператора.</param>
    /// <param name="right">Значение справа от оператора.</param>
    /// <returns>Признак нераветства объектов.</returns>
    public static bool operator !=(Entity<TId> left, Entity<TId> right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    /// Получение хэш-кода.
    /// </summary>
    /// <returns>Хэш-код.</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}