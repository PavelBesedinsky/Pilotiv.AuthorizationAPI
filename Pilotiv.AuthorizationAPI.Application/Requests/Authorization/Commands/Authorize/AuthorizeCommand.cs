using FluentResults;
using MediatR;
using Pilotiv.AuthorizationAPI.Application.Requests.Authorization.Commands.Authorize.Dtos;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Authorization.Commands.Authorize;

/// <summary>
/// Команда авторизации.
/// </summary>
public class AuthorizeCommand : IRequest<Result<AuthorizeCommandResponse>>
{
    /// <summary>
    /// Логин пользователя.
    /// </summary>
    public string Login { get; }

    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    public string Password { get; }

    /// <summary>
    /// IP-адрес пользователя.
    /// </summary>
    public string Ip { get; }

    /// <summary>
    /// Создание команды авторизации.
    /// </summary>
    /// <param name="login">Логин.</param>
    /// <param name="password">Пароль.</param>
    /// <param name="ip">IP-адрес пользователя.</param>
    public AuthorizeCommand(string login, string password, string ip)
    {
        Login = login;
        Password = password;
        Ip = ip;
    }
}