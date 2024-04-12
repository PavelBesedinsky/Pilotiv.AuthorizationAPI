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
    public async Task CommitChangesAsync(User value, CancellationToken cancellationToken)
    {
        using var connection = _dbContext.CreateOpenedConnection();
        var trx = connection.BeginTransaction();

        if (value.VkUser is not null)
        {
            await InvokeAsync(connection, trx, value.VkUser, DomainEventsHandlerAsync, cancellationToken);
        }
        await InvokeAsync(connection, trx, value, DomainEventsHandlerAsync, cancellationToken);
        
        trx.Commit();
    }

    /// <summary>
    /// Обработчик доменного события.
    /// </summary>
    /// <param name="connection">Соденинения.</param>
    /// <param name="transaction">Транзакция.</param>
    /// <param name="domainEvent">Доменное событие</param>
    /// <exception cref="ArgumentOutOfRangeException">Неизвестное доменное событие.</exception>
    private Task DomainEventsHandlerAsync(IDbConnection connection, IDbTransaction transaction,
        IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            UserCreatedDomainEvent dEvent => OnUserCreatedDomainEventAsync(dEvent, connection, transaction),
            UserEmailChangedDomainEvent dEvent => OnUserEmailChangedDomainEventAsync(dEvent, connection, transaction),
            UserRegistrationDateChangedDomainEvent dEvent => OnUserRegistrationDateChangedDomainEventAsync(dEvent,
                connection, transaction),
            UserAuthorizationDateChangedDomainEvent dEvent => OnUserAuthorizationDateChangedAsync(dEvent, connection, transaction),
            UserLoginChangedDomainEvent dEvent => OnUserLoginChangedDomainEventAsync(dEvent, connection, transaction),
            UserVkUserChangedDomainEvent dEvent => OnUserVkUserChangedDomainEventAsync(dEvent, connection, transaction),
            VkUserCreatedDomainEvent dEvent => OnVkUserCreatedDomainEventAsync(dEvent, connection, transaction),
            VkUserInternalIdChangedDomainEvent dEvent => OnVkUserInternalIdChangedDomainEventAsync(dEvent, connection,
                transaction),
            _ => throw new ArgumentOutOfRangeException(nameof(domainEvent))
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
    private static Task OnUserAuthorizationDateChangedAsync(UserAuthorizationDateChangedDomainEvent dEvent, IDbConnection connection,
        IDbTransaction transaction)
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
    /// Обработка события изменения пользователя VK.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private static Task OnUserVkUserChangedDomainEventAsync(UserVkUserChangedDomainEvent dEvent, IDbConnection connection,
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
        return connection.ExecuteAsync(sql, new {Id = dEvent.VkUserId.Value, InternalUserId = dEvent.InternalUserId.Value},
            transaction);
    }
}