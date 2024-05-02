using System.Data;
using Dapper;
using FluentResults;
using Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users;
using Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users.Dtos;
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
            LEFT JOIN refresh_tokens rt on u.id = rt.user_id
            WHERE u.id = @Id";


        var userDao = await QueryUserDaoAsync(connection, sql, new {Id = userId.Value});
        if (userDao is null)
        {
            return UsersErrors.UserNotFoundById(userId);
        }

        return RestoreUserFromDao(userDao);
    }

    /// <inheritdoc />
    public async Task<Result<User>> GetUserByLoginAsync(UserLogin login)
    {
        using var connection = _dbContext.CreateOpenedConnection();
        const string sql = @"SELECT * From users u 
            LEFT JOIN vk_users vku ON u.vk_user_id = vku.id
            LEFT JOIN refresh_tokens rt on u.id = rt.user_id
            WHERE u.login = @Login";

        var userDao = await QueryUserDaoAsync(connection, sql, new {Login = login.Value});
        if (userDao is null)
        {
            return UsersErrors.UserNotFoundByLogin(login);
        }

        return RestoreUserFromDao(userDao);
    }

    /// <inheritdoc />
    public async Task<Result<User>> GetUserByLoginPasswordAsync(UserLogin login, UserPasswordHash password)
    {
        var getUserResult = await GetUserByLoginAsync(login);
        if (getUserResult.IsFailed)
        {
            return getUserResult.ToResult();
        }

        if (getUserResult.ValueOrDefault.PasswordHash is null)
        {
            return UsersErrors.UserPasswordIsIncorrect(getUserResult.ValueOrDefault.Id);
        }

        if (getUserResult.ValueOrDefault.PasswordHash != password)
        {
            return UsersErrors.UserPasswordIsIncorrect(getUserResult.ValueOrDefault.Id);
        }

        return getUserResult;
    }

    /// <inheritdoc />
    public async Task<Result<User>> GetUserByEmailAsync(UserEmail email)
    {
        using var connection = _dbContext.CreateOpenedConnection();
        const string sql = @"SELECT * From users u 
            LEFT JOIN vk_users vku ON u.vk_user_id = vku.id
            LEFT JOIN refresh_tokens rt on u.id = rt.user_id
            WHERE u.email = @Email";

        var userDao = await QueryUserDaoAsync(connection, sql, new {Email = email.Value});
        if (userDao is null)
        {
            return UsersErrors.UserNotFoundByEmail(email);
        }

        return RestoreUserFromDao(userDao);
    }

    /// <inheritdoc />
    public async Task<Result<User>> GetUserByVkInternalIdAsync(VkInternalUserId internalUserId)
    {
        using var connection = _dbContext.CreateOpenedConnection();
        const string sql = @"SELECT * From users u 
            INNER JOIN vk_users vku ON u.vk_user_id = vku.id 
            LEFT JOIN refresh_tokens rt on u.id = rt.user_id   
            WHERE vku.internal_id = @InternalId";

        var userDao = await QueryUserDaoAsync(connection, sql, new {InternalId = internalUserId.Value});
        if (userDao is null)
        {
            return UsersErrors.UserNotFoundByInternalId(internalUserId);
        }

        return RestoreUserFromDao(userDao);
    }

    /// <inheritdoc />
    public async Task<Result<bool>> IsLoginOccupiedAsync(UserLogin login)
    {
        var connection = _dbContext.CreateOpenedConnection();
        const string sql = @"SELECT EXISTS(SELECT 1 FROM users WHERE LOWER(TRIM(login))=@Login)";

        return await connection.ExecuteScalarAsync<bool>(sql, new {Login = login.Value.Trim().ToLower()});
    }

    /// <inheritdoc />
    public async Task<Result<bool>> IsEmailOccupiedAsync(UserEmail email)
    {
        var connection = _dbContext.CreateOpenedConnection();
        const string sql = @"SELECT EXISTS(SELECT 1 FROM users WHERE LOWER(TRIM(email))=@Email)";

        return await connection.ExecuteScalarAsync<bool>(sql, new {Email = email.Value.Trim().ToLower()});
    }

    /// <inheritdoc />
    public async Task<Result<bool>> IsVkUserExistsAsync(VkInternalUserId internalId)
    {
        var connection = _dbContext.CreateOpenedConnection();
        const string sql = @"SELECT EXISTS(SELECT 1 FROM vk_users WHERE internal_id=@Id)";

        return await connection.ExecuteScalarAsync<bool>(sql, new {Id = internalId.Value});
    }

    /// <summary>
    /// Запрос объекта доступа данных пользователя. 
    /// </summary>
    /// <param name="connection">Соединение.</param>
    /// <param name="sql">Описание запроса.</param>
    /// <param name="param">Параметр, по которому фильтруется пользователь.</param>
    /// <returns>Объект доступа данных пользователя.</returns>
    private static async Task<UserDao?> QueryUserDaoAsync(IDbConnection connection, string sql, object? param = null)
    {
        var users = await connection.QueryAsync<UserDao, VkUserDao?, RefreshTokenDao?, UserDao>(sql,
            (user, vkUser, refreshTokenDao) =>
            {
                if (vkUser is not null)
                {
                    user.VkUser = vkUser;
                }

                if (refreshTokenDao is not null)
                {
                    user.RefreshTokens.Add(refreshTokenDao);
                }

                return user;
            }, param);
        
        return users.GroupBy(entity => entity.Id).Select(entity =>
        {
            var user = entity.First();
            user.RefreshTokens = entity.Select(item => item.RefreshTokens.Single()).ToList();
            return user;
        }).SingleOrDefault();
    }

    /// <summary>
    /// Востановление пользователя из объекта доступа.
    /// </summary>
    /// <param name="userDao">Объект доступа данных пользователя.</param>
    /// <returns>Пользователь.</returns>
    private static Result<User> RestoreUserFromDao(UserDao userDao)
    {
        var usersFabric = new UserFactory(new()
        {
            Id = userDao.Id,
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
                },
            RefreshTokens = userDao.RefreshTokens.Select(GetUsersFactoryRefreshTokenPayload).ToList()
        });

        return usersFabric.Restore();
    }

    /// <summary>
    /// Получение объекта переноса данных информации о токене обновления для фабрики пользователей.
    /// </summary>
    /// <param name="refreshTokenDao">Объект доступа данных токена обновления.</param>
    /// <returns>Объект переноса данных информации о токене обновления для фабрики пользователей.</returns>
    private static UsersFactoryRefreshTokenPayload GetUsersFactoryRefreshTokenPayload(RefreshTokenDao refreshTokenDao)
    {
        return new UsersFactoryRefreshTokenPayload(refreshTokenDao.Id ?? string.Empty)
        {
            ExpirationDate = refreshTokenDao.ExpirationDate,
            CreatedDate = refreshTokenDao.CreatedDate,
            RevokedDate = refreshTokenDao.RevokedDate,
            CreatedByIp = refreshTokenDao.CreatedByIp,
            RevokedByIp = refreshTokenDao.RevokedByIp,
            RevokeReason = refreshTokenDao.RevokeReason,
            ReplacingToken = refreshTokenDao.ReplacingToken is not null
                ? GetUsersFactoryRefreshTokenPayload(refreshTokenDao.ReplacingToken)
                : null
        };
    }
}