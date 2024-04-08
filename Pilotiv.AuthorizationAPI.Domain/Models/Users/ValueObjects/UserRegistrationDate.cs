using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Errors;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

/// <summary>
/// Дата регистрации пользователя.
/// </summary>
public class UserRegistrationDate : ValueObject
{
    /// <summary>
    /// Минимальная дата.
    /// </summary>
    private static readonly DateTime MinDate = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Значение даты регистрации пользователя.
    /// </summary>
    public DateTime Value { get; }

    /// <summary>
    /// Создание даты регистрации пользователя.
    /// </summary>
    /// <param name="value">Дата регистрации пользователя.</param>
    private UserRegistrationDate(DateTime value)
    {
        Value = value;
    }

    /// <summary>
    /// Создание даты регистрации пользователя.
    /// </summary>
    /// <param name="value">Значение даты регистрации пользователя.</param>
    /// <returns>Дата регистрации пользователя.</returns>
    public static Result<UserRegistrationDate> Create(DateTime value)
    {
        if (value == DateTime.MinValue)
        {
            return UsersErrors.RegistrationDateIsNotSpecified();
        }
        
        if (value.Kind is DateTimeKind.Unspecified)
        {
            value = new DateTime(value.Ticks, DateTimeKind.Local).ToUniversalTime();
        }

        if (value < MinDate)
        {
            return UsersErrors.InvalidRegistrationDatePeriod(MinDate);
        }

        return new UserRegistrationDate(value);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}