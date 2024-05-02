using System.ComponentModel.DataAnnotations;
using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Errors;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

/// <summary>
/// Адрес электронной почты пользователя.
/// </summary>
public class UserEmail : ValueObject
{
    /// <summary>
    /// Минимальная длина.
    /// </summary>
    private const int MinLength = 1;

    /// <summary>
    /// Максимальная длина.
    /// </summary>
    private const int MaxLength = 255;
    
    /// <summary>
    /// Создание адреса электронной почты пользователя.
    /// </summary>
    /// <param name="email">Адрес электронной почты пользователя.</param>
    private UserEmail(string email)
    {
        Value = email;
    }
    
    /// <summary>
    /// Значение адреса электронной почты.
    /// </summary>
    public string Value { get; }
    
    /// <summary>
    /// Создание адреса электронной почты пользователя.
    /// </summary>
    /// <param name="value">Значение электронной почты пользователя.</param>
    /// <returns>Адрес электронной почты пользователя.</returns>
    public static Result<UserEmail> Create(string value)
    {
        List<IError> errors = new();
        
        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add(UsersErrors.UserEmailIsNullOrEmpty());
        }

        if (value.Length < MinLength)
        {
            errors.Add(UsersErrors.InvalidUserEmail());
        }

        if (value.Length > MaxLength)
        {
            errors.Add(UsersErrors.InvalidUserEmail());
        }

        if (!IsValidEmail(value))
        {
            errors.Add(UsersErrors.InvalidUserEmail());
        }

        if (errors.Any())
        {
            return Result.Fail(errors);
        }
        
        return new UserEmail(value);
    }
    
    /// <summary>
    /// Получение признака, что адрес электронной почты - валидный.
    /// </summary>
    /// <param name="value">Значений адреса электронной почты.</param>
    private static bool IsValidEmail(string value)
    {
        return !string.IsNullOrWhiteSpace(value) && new EmailAddressAttribute().IsValid(value);
    }
    
    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.Trim().ToLower();
    }
}