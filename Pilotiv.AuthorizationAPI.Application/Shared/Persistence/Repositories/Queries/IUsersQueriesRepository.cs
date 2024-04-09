using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

namespace Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Queries;

/// <summary>
/// Интерфейс репозитория запросов доменной модели <see cref="User"/>. 
/// </summary>
public interface IUsersQueriesRepository
{
    /// <summary>
    /// Получение пользователя по логину.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <returns>Пользователь.</returns>
    Task<Result<User>> GetUserByLoginAsync(UserLogin login);
    
    /// <summary>
    /// Получение пользователя по электронному адресу.
    /// </summary>
    /// <param name="email">Адрес электронной почты пользователя.</param>
    /// <returns>Пользователь.</returns>
    Task<Result<User>> GetUserByEmailAsync(UserEmail email);
}