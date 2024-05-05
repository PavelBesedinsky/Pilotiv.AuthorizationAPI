using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Errors;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

/// <summary>
/// Хэш-пароля пользователя.
/// </summary>
public class UserPasswordHash : ValueObject
{
    /// <summary>
    /// Создание хэша-пароля пользователя.
    /// </summary>
    /// <param name="value">Значение хэша-пароля.</param>
    private UserPasswordHash(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Значение хэша-пароля.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Создание хэша-пароля.
    /// </summary>
    /// <param name="value">Значение хэша-пароля.</param>
    /// <returns>Хэш-пароля пользователя.</returns>
    public static Result<UserPasswordHash> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return UsersErrors.UserPasswordHashIsNullOrEmpty;
        }

        return new UserPasswordHash(value);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}