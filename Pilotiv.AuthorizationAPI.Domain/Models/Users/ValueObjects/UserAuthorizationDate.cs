using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Errors;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

/// <summary>
/// Дата авторизации пользователя.
/// </summary>
public class UserAuthorizationDate : ValueObject
{
    /// <summary>
    /// Значение даты авторизации пользователя.
    /// </summary>
    public DateTime Value { get; }

    /// <summary>
    /// Создание даты авторизации пользователя.
    /// </summary>
    /// <param name="value">Дата авторизации пользователя.</param>
    private UserAuthorizationDate(DateTime value)
    {
        Value = value;
    }
    
    /// <summary>
    /// Создание даты авторизации пользователя.
    /// </summary>
    /// <param name="value">Значение даты авторизации пользователя.</param>
    /// <returns>Дата авторизации пользователя.</returns>
    public static Result<UserAuthorizationDate> Create(DateTime value)
    {
        if (value == DateTime.MinValue)
        {
            return UsersErrors.AuthorizationDateIsNotSpecified();
        }
        
        if (value.Kind is DateTimeKind.Unspecified)
        {
            value = new DateTime(value.Ticks, DateTimeKind.Local).ToUniversalTime();
        }
        
        return new UserAuthorizationDate(value);
    }
    
    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}