using Dapper;
using FluentResults;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Queries;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Infrastructure.Daos.Users;
using Pilotiv.AuthorizationAPI.Infrastructure.Fabrics.Users;
using Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Context;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Repositories.Queries;

/// <summary>
/// Репозиторий запросов пользователей.
/// </summary>
public class UsersQueriesRepository : IUsersQueriesRepository
{
    private readonly DbContext _dbContext;

    /// <summary>
    /// Создание репозитория запросов пользователей.
    /// </summary>
    /// <param name="dbContext">Контекст БД.</param>
    public UsersQueriesRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<Result<User>> GetUserByLoginAsync(UserLogin login)
    {
        using var connection = _dbContext.CreateOpenedConnection();
        const string sql = @"SELECT * From Users WHERE Login = @Login";
        
        var userDao = await connection.QuerySingleOrDefaultAsync<UserDao>(sql, new {Login = login.Value});
        if (userDao is null)
        {
            throw new NotImplementedException();
        }

        var usersFabric = new UsersFabric(userDao);
        return usersFabric.Restore(userDao.Id);
    }

    /// <inheritdoc />
    public async Task<Result<User>> GetUserByEmailAsync(UserEmail email)
    {
        using var connection = _dbContext.CreateOpenedConnection();
        const string sql = @"SELECT * From Users WHERE Email = @Email";
        
        var userDao = await connection.QuerySingleOrDefaultAsync<UserDao>(sql, new {Email = email.Value});
        if (userDao is null)
        {
            throw new NotImplementedException();
        }

        var usersFabric = new UsersFabric(userDao);
        return usersFabric.Restore(userDao.Id);
    }
}