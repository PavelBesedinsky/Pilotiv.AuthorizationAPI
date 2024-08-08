using FluentResults;
using MediatR;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Registration.Commands.Register;

/// <summary>
/// Команда регистрации.
/// </summary>
public class RegisterCommand : IRequest<Result>
{
    /// <summary>
    /// Логин.
    /// </summary>
    public string Login { get; }
    
    /// <summary>
    /// Пароль.
    /// </summary>
    public string Password { get; }
    
    /// <summary>
    /// Адрес электронной почты.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// Создание команды регистрации.
    /// </summary>
    /// <param name="login">Логин.</param>
    /// <param name="password">Пароль.</param>
    /// <param name="email">Адрес электронной почты.</param>
    public RegisterCommand(string login, string password, string email)
    {
        Login = login;
        Password = password;
        Email = email;
    }
}