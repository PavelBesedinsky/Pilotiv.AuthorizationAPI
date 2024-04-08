using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

/// <summary>
/// Идентификатор пользователя VK.
/// </summary>
public class VkUserId : AggregateRootId<Guid>
{
    /// <summary>
    /// Создание идентификатор пользователя VK.
    /// </summary>
    /// <param name="value">Значение идентификатора.</param>
    private VkUserId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Значение идентификатора VK.
    /// </summary>
    public override Guid Value { get; }

    /// <summary>
    /// Создание идентификатора пользователя VK.
    /// </summary>
    /// <returns>Идентификатор пользователя.</returns>
    public static VkUserId Create()
    {
        return new VkUserId(Guid.NewGuid());
    }

    /// <summary>
    /// Создание идентификатора пользователя VK.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Идентификатор пользователя.</returns>
    public static VkUserId Create(Guid id)
    {
        return new VkUserId(id);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}