using FluentResults;
using MediatR;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Queries;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Users.Queries.GetUserById;

/// <summary>
/// Обработчик команды получения пользователя по идентификатору.
/// </summary>
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<User>>
{
    private readonly IUsersQueriesRepository _usersQueriesRepository;

    /// <summary>
    /// Создание обработчика команды получения пользователя по идентификатору.
    /// </summary>
    /// <param name="usersQueriesRepository">Репозиторий запросов пользователя.</param>
    public GetUserByIdQueryHandler(IUsersQueriesRepository usersQueriesRepository)
    {
        _usersQueriesRepository = usersQueriesRepository;
    }
    
    /// <summary>
    /// Обработка команды получения пользователя по идентификатору.
    /// </summary>
    /// <param name="request">Команда получения пользователя по идентификатору.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Пользователь.</returns>
    public Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken = default)
    {
        return _usersQueriesRepository.GetUserByIdAsync(UserId.Create(request.Id));;
    }
}