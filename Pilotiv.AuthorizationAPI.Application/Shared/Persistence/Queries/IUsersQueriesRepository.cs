using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

namespace Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Queries;

/// <summary>
/// Интерфейс репозитория запросов доменной модели <see cref="User"/>. 
/// </summary>
public interface IUsersQueriesRepository
{
    /// <summary>
    /// Получение пользователя по логину.
    /// </summary>
    /// <returns>Пользователь.</returns>
    Task<Result<User>> GetUserByLoginAsync();
    
    /// <summary>
    /// Получение пользователя по электронному адресу.
    /// </summary>
    /// <returns>Пользователь.</returns>
    Task<Result<User>> GetUserByEmailAsync(UserEmail email);
}