using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Errors;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

/// <summary>
/// Пароль пользователя.
/// </summary>
public class UserPassword : ValueObject
{
    /// <summary>
    /// Создание пароля пользователя.
    /// </summary>
    /// <param name="hash">Значение хэша-пароля.</param>
    /// <param name="salt">Соль.</param>
    private UserPassword(string hash, string salt)
    {
        Hash = hash;
        Salt = salt;
    }

    /// <summary>
    /// Хэш.
    /// </summary>
    public string Hash { get; }

    /// <summary>
    /// Соль.
    /// </summary>
    public string Salt { get; }

    /// <summary>
    /// Создание пароля.
    /// </summary>
    /// <param name="hash">Значение хэша-пароля.</param>
    /// <param name="salt">Соль.</param>
    /// <returns>Пароль пользователя.</returns>
    public static Result<UserPassword> Create(string hash, string salt)
    {
        if (string.IsNullOrWhiteSpace(hash))
        {
            return UsersErrors.UserPasswordHashIsNullOrEmpty;
        }

        return new UserPassword(hash, salt);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Hash;
        yield return Salt;
    }
}