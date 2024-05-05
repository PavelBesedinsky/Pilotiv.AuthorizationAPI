using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

/// <summary>
/// Идентификатор токена доступа.
/// </summary>
public class RefreshTokenId : ValueObject
{
    /// <summary>
    /// Создание идентификатор пользователя VK.
    /// </summary>
    /// <param name="value">Значение идентификатора.</param>
    private RefreshTokenId(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Значение идентификатора VK.
    /// </summary>
    public string Value { get; }
    
    /// <summary>
    /// Создание идентификатора пользователя VK.
    /// </summary>
    /// <param name="value">Значение токена.</param>
    /// <returns>Идентификатор пользователя.</returns>
    public static RefreshTokenId Create(string value)
    {
        return new RefreshTokenId(value);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}