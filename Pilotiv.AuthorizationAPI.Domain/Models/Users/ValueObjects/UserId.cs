using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

/// <summary>
/// Идентификатор пользователя.
/// </summary>
public class UserId : AggregateRootId<Guid>
{
    /// <summary>
    /// Создание идентификатор пользователя.
    /// </summary>
    /// <param name="value">Значение идентификатора.</param>
    private UserId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Значение идентификатора.
    /// </summary>
    public override Guid Value { get; }
    
    /// <summary>
    /// Создание идентификатора пользователя.
    /// </summary>
    /// <returns>Идентификатор пользователя.</returns>
    public static UserId Create()
    {
        return new UserId(Guid.NewGuid());
    }

    /// <summary>
    /// Создание идентификатора пользователя.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Идентификатор пользователя.</returns>
    public static UserId Create(Guid id)
    {
        return new UserId(id);
    }
    
    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}