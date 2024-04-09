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
    public Task CommitChangesAsync(User value, CancellationToken cancellationToken)
    {
        return InvokeAsync(_dbContext, value, DomainEventsHandlerAsync, cancellationToken);
    }

    /// <summary>
    /// Обработчик доменного события.
    /// </summary>
    /// <param name="connection">Соденинения.</param>
    /// <param name="transaction">Транзакция.</param>
    /// <param name="domainEvent">Доменное событие</param>
    private Task DomainEventsHandlerAsync(IDbConnection connection, IDbTransaction transaction,
        IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            UserCreatedDomainEvent dEvent => OnUserCreatedDomainEventAsync(dEvent, connection, transaction),
            UserEmailChangedDomainEvent dEvent => OnUserEmailChangedDomainEventAsync(dEvent, connection, transaction),
            UserRegistrationDateChangedDomainEvent dEvent => OnUserRegistrationDateChangedDomainEventAsync(dEvent,
                connection, transaction),
            UserAuthorizationDateChanged dEvent => OnUserAuthorizationDateChangedAsync(dEvent, connection, transaction),
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
    private Task OnUserCreatedDomainEventAsync(UserCreatedDomainEvent dEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        const string sql = @"INSERT INTO Users (Id) VALUES(@Id)";
        return connection.ExecuteAsync(sql, new {Id = dEvent.UserId.Value}, transaction);
    }

    /// <summary>
    /// Обработка события изменения адреса электронной почты пользователя.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private Task OnUserEmailChangedDomainEventAsync(UserEmailChangedDomainEvent dEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        const string sql = @"UPDATE Users SET Email = @Email WHERE Id = @Id";
        return connection.ExecuteAsync(sql, new {Id = dEvent.UserId.Value, Email = dEvent.UserEmail.Value},
            transaction);
    }

    /// <summary>
    /// Обработка события изменения даты регистрации пользователя.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private Task OnUserRegistrationDateChangedDomainEventAsync(UserRegistrationDateChangedDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE Users SET RegistrationDate = @RegistrationDate WHERE Id = @Id";
        return connection.ExecuteAsync(sql, new {Id = dEvent.UserId.Value, Login = dEvent.RegistrationDate},
            transaction);
    }

    /// <summary>
    /// Обработка события изменения даты авторизации пользователя.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private Task OnUserAuthorizationDateChangedAsync(UserAuthorizationDateChanged dEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        const string sql = @"UPDATE Users SET AuthorizationDate = @AuthorizationDate WHERE Id = @Id";
        return connection.ExecuteAsync(sql, new {Id = dEvent.UserId.Value, Login = dEvent.AuthorizationDate},
            transaction);
    }

    /// <summary>
    /// Обработка события изменения логина пользователя.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private Task OnUserLoginChangedDomainEventAsync(UserLoginChangedDomainEvent dEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        const string sql = @"UPDATE Users SET Login = @Login WHERE Id = @Id";
        return connection.ExecuteAsync(sql, new {Id = dEvent.UserId.Value, Login = dEvent.Login.Value},
            transaction);
    }

    /// <summary>
    /// Обработка события изменения пользователя VK.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private Task OnUserVkUserChangedDomainEventAsync(UserVkUserChangedDomainEvent dEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        const string sql = @"UPDATE Users SET VkUserId = @VkUserId WHERE Id = @Id";
        return connection.ExecuteAsync(sql, new {Id = dEvent.UserId.Value, VkUserId = dEvent.VkUser.Id.Value},
            transaction);
    }

    /// <summary>
    /// Обработка события создания пользователя VK.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private Task OnVkUserCreatedDomainEventAsync(VkUserCreatedDomainEvent dEvent, IDbConnection connection,
        IDbTransaction transaction)
    {
        const string sql = @"INSERT INTO VkUsers (Id) VALUES(@Id)";
        return connection.ExecuteAsync(sql, new {Id = dEvent.Id.Value}, transaction);
    }

    /// <summary>
    /// Обработка события изменения внутреннего идентификатор пользователя в VK.
    /// </summary>
    /// <param name="dEvent">Событие.</param>
    /// <param name="connection">Соединение.</param>
    /// <param name="transaction">Транзакция.</param>
    private Task OnVkUserInternalIdChangedDomainEventAsync(VkUserInternalIdChangedDomainEvent dEvent,
        IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"UPDATE VkUsers SET InternalUserId = @InternalUserId WHERE Id = @Id";
        return connection.ExecuteAsync(sql, new {Id = dEvent.VkUserId.Value, VkUserId = dEvent.InternalUserId.Value},
            transaction);
    }
}