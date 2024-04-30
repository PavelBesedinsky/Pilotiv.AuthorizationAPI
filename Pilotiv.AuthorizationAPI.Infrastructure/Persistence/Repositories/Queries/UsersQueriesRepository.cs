using Dapper;
using FluentResults;
using Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Queries;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Infrastructure.Daos.Users;
using Pilotiv.AuthorizationAPI.Infrastructure.Errors.Users;
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
    public async Task<Result<User>> GetUserByIdAsync(UserId userId)
    {
        using var connection = _dbContext.CreateOpenedConnection();
        const string sql = @"SELECT * From users u 
            LEFT JOIN vk_users vku ON u.vk_user_id = vku.id 
            WHERE u.id = @Id";

        var userDao = (await connection.QueryAsync<UserDao, VkUserDao, UserDao>(sql, (user, vkUser) =>
        {
            if (vkUser.Id != Guid.Empty)
            {
                user.VkUser = vkUser;
            }

            return user;
        }, new {Id = userId.Value}, splitOn: @"vk_user_id")).SingleOrDefault();
        if (userDao is null)
        {
            return UsersErrors.UserNotFoundById(userId);
        }

        var usersFabric = new UsersFabric(new()
        {
            PasswordHash = userDao.PasswordHash,
            Email = userDao.Email,
            Login = userDao.Login,
            RegistrationDate = userDao.RegistrationDate,
            AuthorizationDate = userDao.AuthorizationDate,
            VkUser = userDao.VkUser is null
                ? null
                : new()
                {
                    InternalUserId = userDao.VkUser.InternalId
                }
        });

        return usersFabric.Restore(userDao.Id);
    }

    /// <inheritdoc />
    public async Task<Result<User>> GetUserByLoginAsync(UserLogin login)
    {
        using var connection = _dbContext.CreateOpenedConnection();
        const string sql = @"SELECT * From users u 
            LEFT JOIN vk_users vku ON u.vk_user_id = vku.id 
            WHERE u.login = @Login";

        var userDao = (await connection.QueryAsync<UserDao, VkUserDao, UserDao>(sql, (user, vkUser) =>
        {
            if (vkUser.Id != Guid.Empty)
            {
                user.VkUser = vkUser;
            }

            return user;
        }, new {Login = login.Value}, splitOn: @"vk_user_id")).SingleOrDefault();
        if (userDao is null)
        {
            return UsersErrors.UserNotFoundByLogin(login);
        }

        var usersFabric = new UsersFabric(new()
        {
            PasswordHash = userDao.PasswordHash,
            Email = userDao.Email,
            Login = userDao.Login,
            RegistrationDate = userDao.RegistrationDate,
            AuthorizationDate = userDao.AuthorizationDate,
            VkUser = userDao.VkUser is null
                ? null
                : new()
                {
                    InternalUserId = userDao.VkUser.InternalId
                }
        });

        return usersFabric.Restore(userDao.Id);
    }

    /// <inheritdoc />
    public async Task<Result<User>> GetUserByEmailAsync(UserEmail email)
    {
        using var connection = _dbContext.CreateOpenedConnection();
        const string sql = @"SELECT * From users u 
            LEFT JOIN vk_users vku ON u.vk_user_id = vku.id 
            WHERE u.email = @Email";

        var userDao = (await connection.QueryAsync<UserDao, VkUserDao, UserDao>(sql, (user, vkUser) =>
        {
            if (vkUser.Id != Guid.Empty)
            {
                user.VkUser = vkUser;
            }

            return user;
        }, new {Email = email.Value}, splitOn: @"vk_user_id")).SingleOrDefault();
        if (userDao is null)
        {
            return UsersErrors.UserNotFoundByEmail(email);
        }

        var usersFabric = new UsersFabric(new()
        {
            PasswordHash = userDao.PasswordHash,
            Email = userDao.Email,
            Login = userDao.Login,
            RegistrationDate = userDao.RegistrationDate,
            AuthorizationDate = userDao.AuthorizationDate,
            VkUser = userDao.VkUser is null
                ? null
                : new()
                {
                    Id = userDao.VkUser.Id,
                    InternalUserId = userDao.VkUser.InternalId
                }
        });

        return usersFabric.Restore(userDao.Id);
    }

    /// <inheritdoc />
    public async Task<Result<User>> GetUserByVkInternalIdAsync(VkInternalUserId internalUserId)
    {
        using var connection = _dbContext.CreateOpenedConnection();
        const string sql = @"SELECT * From users u 
            INNER JOIN vk_users vku ON u.vk_user_id = vku.id 
            WHERE vku.internal_id = @InternalId";

        var userDao = (await connection.QueryAsync<UserDao, VkUserDao, UserDao>(sql, (user, vkUser) =>
        {
            if (vkUser.Id != Guid.Empty)
            {
                user.VkUser = vkUser;
            }

            return user;
        }, new {InternalId = internalUserId.Value}, splitOn: @"vk_user_id")).SingleOrDefault();
        if (userDao is null)
        {
            return UsersErrors.UserNotFoundByInternalId(internalUserId);
        }

        var usersFabric = new UsersFabric(new()
        {
            PasswordHash = userDao.PasswordHash,
            Email = userDao.Email,
            Login = userDao.Login,
            RegistrationDate = userDao.RegistrationDate,
            AuthorizationDate = userDao.AuthorizationDate,
            VkUser = userDao.VkUser is null
                ? null
                : new()
                {
                    Id = userDao.VkUser.Id,
                    InternalUserId = userDao.VkUser.InternalId
                }
        });

        return usersFabric.Restore(userDao.Id);
    }

    /// <inheritdoc />
    public async Task<Result<bool>> IsLoginOccupiedAsync(UserLogin login)
    {
        var connection = _dbContext.CreateOpenedConnection();
        const string sql = @"SELECT EXISTS(SELECT 1 FROM users WHERE login=@Login)";

        return await connection.ExecuteScalarAsync<bool>(sql, new {Login = login.Value});
    }

    /// <inheritdoc />
    public async Task<Result<bool>> IsEmailOccupiedAsync(UserEmail email)
    {
        var connection = _dbContext.CreateOpenedConnection();
        const string sql = @"SELECT EXISTS(SELECT 1 FROM users WHERE email=@Email)";

        return await connection.ExecuteScalarAsync<bool>(sql, new {Email = email.Value});
    }

    /// <inheritdoc />
    public async Task<Result<bool>> IsVkUserExistsAsync(VkInternalUserId internalId)
    {
        var connection = _dbContext.CreateOpenedConnection();
        const string sql = @"SELECT EXISTS(SELECT 1 FROM vk_users WHERE internal_id=@Id)";

        return await connection.ExecuteScalarAsync<bool>(sql, new {Id = internalId.Value});
    }
}