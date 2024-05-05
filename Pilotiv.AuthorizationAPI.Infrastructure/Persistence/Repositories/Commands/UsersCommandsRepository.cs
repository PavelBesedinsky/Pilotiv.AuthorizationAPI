using System.Data;
using Dapper;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Commands;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;
using Pilotiv.AuthorizationAPI.Domain.Primitives;
using Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Context;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Repositories.Commands;

/// <summary>
/// Репозиторий команд пользователей.
/// </summary>
public class UsersCommandsRepository : BaseCommandsRepository, IUsersCommandsRepository
{
    private readonly DbContext _dbContext;

    /// <summary>
    /// Создание репозитория команд пользователей.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    public UsersCommandsRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task CommitChangesAsync(User user, CancellationToken cancellationToken)
    {
        using var connection = _dbContext.CreateOpenedConnection();
        var trx = connection.BeginTransaction();

        if (user.VkUser is not null)
        {
            await InvokeAsync(connection, trx, user.VkUser, VkUserDomainEventsHandlerAsync, cancellationToken);
        }

        foreach (var refreshToken in user.RefreshTokens.Reverse())
        {
            await InvokeAsync(connection, trx, refreshToken, RefreshTokenDomainEventsHandlerAsync, cancellationToken);
        }

        await InvokeAsync(connection, trx, user, UserDomainEventsHandlerAsync, cancellationToken);

        trx.Commit();
    }

    /// <summary>
    /// Обработчик доменного события пользователя VK.
    /// </summary>
    /// <param name="connection">Соденинения.</param>
    /// <param name="transaction">Транзакция.</param>
    /// <param name="domainEvent">Доменное событие</param>
    /// <exception cref="ArgumentOutOfRangeException">Неизвестное доменное событие.</exception>
    private static Task VkUserDomainEventsHandlerAsync(IDbConnection connection, IDbTransaction transaction,
        IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            VkUserCreatedDomainEvent dEvent => OnVkUserCreatedDomainEventAsync(dEvent, connection, transaction),
            VkUserInternalIdChangedDomainEvent dEvent => OnVkUserInternalIdChangedDomainEventAsync(dEvent, connection, transaction),
            _ => Task.CompletedTask
        };
    }

    /// <summary>
    /// Обработчик доменного события токена обновления.
    /// </summary>
    /// <param name="connection">Соденинения.</param>
    /// <param name="transaction">Транзакция.</param>
    /// <param name="domainEvent">Доменное событие</param>
    /// <exception cref="ArgumentOutOfRangeException">Неизвестное доменное событие.</exception>
    private static Task RefreshTokenDomainEventsHandlerAsync(IDbConnection connection, IDbTransaction transaction,
        IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            RefreshTokenCreatedDomainEvent dEvent => OnRefreshTokenCreatedDomainEventAsync(dEvent, connection, transaction),
            RefreshTokenCreatedByIpChangedDomainEvent dEvent => OnRefreshTokenCreatedByIpChangedDomainEventAsync(dEvent, connection, transaction),
            RefreshTokenCreatedDateChangedDomainEvent dEvent => OnRefreshTokenCreatedDateChangedDomainEventAsync(dEvent, connection, transaction),
            RefreshTokenExpirationDateChangedDomainEvent dEvent => OnRefreshTokenExpirationDateChangedDomainEventAsync(dEvent, connection, transaction),
            RefreshTokenReplacingTokenChangedDomainEvent dEvent => OnRefreshTokenReplacingTokenChangedDomainEventAsync(dEvent, connection, transaction),
            RefreshTokenRevokedByIpChangedDomainEvent dEvent => OnRefreshTokenRevokedByIpChangedDomainEventAsync(dEvent, connection, transaction),
            RefreshTokenRevokedDateChangedDomainEvent dEvent => OnRefreshTokenRevokedDateChangedDomainEventAsync(dEvent, connection, transaction),
            RefreshTokenRevokeReasonChangedDomainEvent dEvent => OnRefreshTokenRevokeReasonChangedDomainEventAsync(dEvent, connection, transaction),
            _ => Task.CompletedTask
        };
    }

    /// <summary>
    /// Обработчик доменного события пользователя.
    /// </summary>
    /// <param name="connection">Соденинения.</param>
    /// <param name="transaction">Транзакция.</param>
    /// <param name="domainEvent">Доменное событие</param>
    /// <exception cref="ArgumentOutOfRangeException">Неизвестное доменное событие.</exception>
    private static Task UserDomainEventsHandlerAsync(IDbConnection connection, IDbTransaction transaction,
        IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            UserCreatedDomainEvent dEvent => OnUserCreatedDomainEventAsync(dEvent, connection, transaction),
            UserPasswordHashChangedDomainEvent dEvent => OnUserPasswordHashChangedDomainEventAsync(dEvent, connection, transaction),
            UserEmailChangedDomainEvent dEvent => OnUserEmailChangedDomainEventAsync(dEvent, connection, transaction),
            UserRegistrationDateChangedDomainEvent dEvent => OnUserRegistrationDateChangedDomainEventAsync(dEvent, connection, transaction),
            UserAuthorizationDateChangedDomainEvent dEvent => OnUserAuthorizationDateChangedAsync(dEvent, connection, transaction),
            UserLoginChangedDomainEvent dEvent => OnUserLoginChangedDomainEventAsync(dEvent, connection, transaction),
            UserRefreshTokenAddedDomainEvent dEvent => OnUserRefreshTokenAddedDomainEventAsync(dEvent, connection, transaction),
            UserVkUserChangedDomainEvent dEvent => OnUserVkUserChangedDomainEventAsync(dEvent, connection, transaction),
            _ => Task.CompletedTask
        };
    }

    /// <summary>
    /// Обработка события создания пользователя.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnUserCreatedDomainEventAsync(UserCreatedDomainEvent dEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        const string sql = @"INSERT INTO users (id) VALUES(@Id)";
        return connection.ExecuteAsync(sql, new {Id = dEvent.UserId.Value}, transaction);
    }

    /// <summary>
    /// Обработка события изменения хэша-пароля пользователя.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnUserPasswordHashChangedDomainEventAsync(UserPasswordHashChangedDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE users SET password_hash = @Password WHERE id = @Id";
        return connection.ExecuteAsync(sql, new {Id = dEvent.UserId.Value, Password = dEvent.PasswordHash.Value},
            transaction);
    }

    /// <summary>
    /// Обработка события изменения адреса электронной почты пользователя.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnUserEmailChangedDomainEventAsync(UserEmailChangedDomainEvent dEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        const string sql = @"UPDATE users SET email = @Email WHERE id = @Id";
        return connection.ExecuteAsync(sql, new {Id = dEvent.UserId.Value, Email = dEvent.UserEmail.Value},
            transaction);
    }

    /// <summary>
    /// Обработка события изменения даты регистрации пользователя.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnUserRegistrationDateChangedDomainEventAsync(UserRegistrationDateChangedDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE users SET registration_date = @RegistrationDate WHERE id = @Id";
        return connection.ExecuteAsync(sql,
            new {Id = dEvent.UserId.Value, RegistrationDate = dEvent.RegistrationDate.Value},
            transaction);
    }

    /// <summary>
    /// Обработка события изменения даты авторизации пользователя.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnUserAuthorizationDateChangedAsync(UserAuthorizationDateChangedDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE users SET authorization_date = @AuthorizationDate WHERE id = @Id";
        return connection.ExecuteAsync(sql,
            new {Id = dEvent.UserId.Value, AuthorizationDate = dEvent.AuthorizationDate.Value},
            transaction);
    }

    /// <summary>
    /// Обработка события изменения логина пользователя.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnUserLoginChangedDomainEventAsync(UserLoginChangedDomainEvent dEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        const string sql = @"UPDATE users SET login = @Login WHERE id = @Id";
        return connection.ExecuteAsync(sql, new {Id = dEvent.UserId.Value, Login = dEvent.Login.Value},
            transaction);
    }

    /// <summary>
    /// Обработка события добавления токена обновления к пользователю.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnUserRefreshTokenAddedDomainEventAsync(UserRefreshTokenAddedDomainEvent dEvent,
        IDbConnection connection,
        IDbTransaction transaction)
    {
        const string sql = @"UPDATE refresh_tokens SET user_id = @UserId WHERE id = @Id";
        return connection.ExecuteAsync(sql, new {Id = dEvent.RefreshToken.Id.Value, UserId = dEvent.UserId.Value},
            transaction);
    }

    /// <summary>
    /// Обработка события изменения пользователя VK.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnUserVkUserChangedDomainEventAsync(UserVkUserChangedDomainEvent dEvent,
        IDbConnection connection,
        IDbTransaction transaction)
    {
        const string sql = @"UPDATE users SET vk_user_id = @VkUserId WHERE id = @Id";
        return connection.ExecuteAsync(sql, new {Id = dEvent.UserId.Value, VkUserId = dEvent.VkUser.Id.Value},
            transaction);
    }

    /// <summary>
    /// Обработка события создания пользователя VK.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnVkUserCreatedDomainEventAsync(VkUserCreatedDomainEvent dEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        const string sql = @"INSERT INTO vk_users (id) VALUES(@Id)";
        return connection.ExecuteAsync(sql, new {Id = dEvent.Id.Value}, transaction);
    }

    /// <summary>
    /// Обработка события изменения внутреннего идентификатор пользователя в VK.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnVkUserInternalIdChangedDomainEventAsync(VkUserInternalIdChangedDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE vk_users SET internal_id = @InternalUserId WHERE Id = @Id";
        return connection.ExecuteAsync(sql,
            new {Id = dEvent.VkUserId.Value, InternalUserId = dEvent.InternalUserId.Value},
            transaction);
    }

    /// <summary>
    /// обработка события создания токена обновления.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnRefreshTokenCreatedDomainEventAsync(RefreshTokenCreatedDomainEvent dEvent,
        IDbConnection connection,
        IDbTransaction transaction)
    {
        const string sql = @"INSERT INTO refresh_tokens (id) VALUES(@Id)";
        return connection.ExecuteAsync(sql, new {Id = dEvent.RefreshTokenId.Value}, transaction);
    }

    /// <summary>
    /// Обработка события изменения IP-адреса пользователя, запрашивающего создание токена.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnRefreshTokenCreatedByIpChangedDomainEventAsync(
        RefreshTokenCreatedByIpChangedDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE refresh_tokens SET created_by_ip = @CreatedByIp WHERE Id = @Id";
        return connection.ExecuteAsync(sql, new {Id = dEvent.RefreshTokenId.Value, CreatedByIp = dEvent.Ip},
            transaction);
    }

    /// <summary>
    /// Обработка события изменения даты создания токена.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnRefreshTokenCreatedDateChangedDomainEventAsync(
        RefreshTokenCreatedDateChangedDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE refresh_tokens SET created_date = @CreatedDate WHERE Id = @Id";
        return connection.ExecuteAsync(sql, new {Id = dEvent.RefreshTokenId.Value, dEvent.CreatedDate}, transaction);
    }

    /// <summary>
    /// Обработка события изменения даты истечения токена.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnRefreshTokenExpirationDateChangedDomainEventAsync(
        RefreshTokenExpirationDateChangedDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE refresh_tokens SET expiration_date = @ExpirationDate WHERE Id = @Id";
        return connection.ExecuteAsync(sql, new {Id = dEvent.RefreshTokenId.Value, dEvent.ExpirationDate}, transaction);
    }

    /// <summary>
    /// Обработка события изменения токена обновления, замеяющего токен.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnRefreshTokenReplacingTokenChangedDomainEventAsync(
        RefreshTokenReplacingTokenChangedDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE refresh_tokens SET replacing_token_id = @ReplacingTokenId WHERE Id = @Id";
        return connection.ExecuteAsync(sql,
            new {Id = dEvent.RefreshTokenId.Value, ReplacingTokenId = dEvent.ReplacingToken.Id.Value}, transaction);
    }

    /// <summary>
    /// Обработка события изменения IP-адреса пользователя, запрашивающего отзыв токена.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnRefreshTokenRevokedByIpChangedDomainEventAsync(
        RefreshTokenRevokedByIpChangedDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE refresh_tokens SET revoked_by_ip = @RevokedByIp WHERE Id = @Id";
        return connection.ExecuteAsync(sql, new {Id = dEvent.RefreshTokenId.Value, RevokedByIp = dEvent.Ip},
            transaction);
    }

    /// <summary>
    /// Обработка события изменения даты отзыва токена.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnRefreshTokenRevokedDateChangedDomainEventAsync(
        RefreshTokenRevokedDateChangedDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE refresh_tokens SET revoked_date = @RevokedDate WHERE Id = @Id";
        return connection.ExecuteAsync(sql, new {Id = dEvent.RefreshTokenId.Value, dEvent.RevokedDate}, transaction);
    }

    /// <summary>
    /// Обработка события изменения причины отзыва токена обновления.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnRefreshTokenRevokeReasonChangedDomainEventAsync(
        RefreshTokenRevokeReasonChangedDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE refresh_tokens SET revoke_reason = @RevokeReason WHERE Id = @Id";
        return connection.ExecuteAsync(sql, new {Id = dEvent.RefreshTokenId.Value, RevokeReason = dEvent.Reason},
            transaction);
    }
}