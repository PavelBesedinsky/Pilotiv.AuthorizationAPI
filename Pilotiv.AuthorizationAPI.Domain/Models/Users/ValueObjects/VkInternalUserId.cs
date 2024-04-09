using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

/// <summary>
/// Внутренний идентификатор пользователя в VK.
/// </summary>
public class VkInternalUserId : ValueObject
{
    /// <summary>
    /// Создание внутреннего идентификатор пользователя в VK.
    /// </summary>
    /// <param name="value">Значение идентификатора.</param>
    private VkInternalUserId(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Значение внутреннего идентификатор пользователя в VK.
    /// </summary>
    public string Value { get; }
    
    /// <summary>
    /// Создание внутреннего идентификатор пользователя в VK.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Внутренний идентификатор пользователя в VK.</returns>
    public static Result<VkInternalUserId> Create(string id)
    {
        // TODO: Добавить ошибку, если идентификатор пустой.
        return new VkInternalUserId(id);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}