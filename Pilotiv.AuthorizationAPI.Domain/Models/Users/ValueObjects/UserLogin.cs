using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Errors;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

/// <summary>
/// Логин пользователя.
/// </summary>
public class UserLogin : ValueObject
{
    /// <summary>
    /// Минимальная длина.
    /// </summary>
    private const int MinLength = 5;

    /// <summary>
    /// Максимальная длина.
    /// </summary>
    private const int MaxLength = 255;
    
    /// <summary>
    /// Значение логина пользователя.
    /// </summary>
    public string Value { get; }
    
    /// <summary>
    /// Создание логина пользователя.
    /// </summary>
    /// <param name="login">Значение логина пользователя.</param>
    private UserLogin(string login)
    {
        Value = login;
    }
    
    /// <summary>
    /// Создание логина пользователя.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <returns>Логин пользователя.</returns>
    public static Result<UserLogin> Create(string value)
    {
        List<IError> errors = new();
        
        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add(UsersErrors.UserLoginIsNullOrEmpty());
        }

        if (value.Length < MinLength)
        {
            errors.Add(UsersErrors.InvalidUserLoginLength(MinLength, MaxLength));
        }

        if (value.Length > MaxLength)
        {
            errors.Add(UsersErrors.InvalidUserLoginLength(MinLength, MaxLength));
        }
        
        if (errors.Any())
        {
            return Result.Fail(errors);
        }
        
        return new UserLogin(value);
    }
    
    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}