using FluentResults;
using MediatR;
using Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.Register.Errors;
using Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users;
using Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users.Dtos;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Commands;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Queries;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.Register;

/// <summary>
/// Обработчик команды регистрации.
/// </summary>
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
{
    private readonly IUsersCommandsRepository _usersCommandsRepository;
    private readonly IUsersQueriesRepository _usersQueriesRepository;

    /// <summary>
    /// Создание обработчика команды регистрации.
    /// </summary>
    /// <param name="usersCommandsRepository">Интерфейс репозитория команд пользователей.</param>
    /// <param name="usersQueriesRepository">Интерфейс репозитория запросов пользователей.</param>
    public RegisterCommandHandler(IUsersCommandsRepository usersCommandsRepository,
        IUsersQueriesRepository usersQueriesRepository)
    {
        _usersCommandsRepository = usersCommandsRepository;
        _usersQueriesRepository = usersQueriesRepository;
    }

    /// <summary>
    /// Обработка команды регистрации.
    /// </summary>
    /// <param name="request">Команда регистрации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task<Result> Handle(RegisterCommand request,
        CancellationToken cancellationToken = default)
    {
        List<IError> errors = new();

        var validateUserLoginAsync = ValidateUserLoginAsync(request.Login);
        var validateUserEmailAsync = ValidateUserEmailAsync(request.Email);

        var validateUserLoginResult = await validateUserLoginAsync;
        if (validateUserLoginResult.IsFailed)
        {
            errors.AddRange(validateUserLoginResult.Errors);
        }

        var validateUserEmailResult = await validateUserEmailAsync;
        if (validateUserEmailResult.IsFailed)
        {
            errors.AddRange(validateUserEmailResult.Errors);
        }

        if (errors.Any())
        {
            return Result.Fail(errors);
        }

        var usersFabric = new UserFactory(new UsersFactoryUserPayload
        {
            PasswordHash = request.Password,
            Email = request.Email,
            Login = request.Login,
            RegistrationDate = DateTime.UtcNow,
            AuthorizationDate = DateTime.UtcNow
        });

        var createUserResult = usersFabric.Create();
        if (createUserResult.IsFailed)
        {
            return createUserResult.ToResult();
        }

        await _usersCommandsRepository.CommitChangesAsync(createUserResult.Value, cancellationToken);

        return Result.Ok();
    }

    /// <summary>
    /// Валидация логина пользователя.
    /// </summary>
    /// <param name="login">Логин.</param>
    private async Task<Result> ValidateUserLoginAsync(string login)
    {
        var getUserLoginResult = UserLogin.Create(login);
        if (getUserLoginResult.IsFailed)
        {
            return Result.Fail(getUserLoginResult.Errors);
        }

        var isLoginOccupiedResult =
            await _usersQueriesRepository.IsLoginOccupiedAsync(getUserLoginResult.ValueOrDefault);
        if (isLoginOccupiedResult.IsFailed)
        {
            return Result.Fail(isLoginOccupiedResult.Errors);
        }

        if (isLoginOccupiedResult.ValueOrDefault)
        {
            return RegisterErrors.LoginIsOccupied(getUserLoginResult.ValueOrDefault);
        }
        
        return Result.Ok();
    }

    /// <summary>
    /// Валидация адреса электронной почты пользователя.
    /// </summary>
    /// <param name="email">Адрес электронной почты пользователя.</param>
    private async Task<Result> ValidateUserEmailAsync(string email)
    {
        var getUserEmailResult = UserEmail.Create(email);
        if (getUserEmailResult.IsFailed)
        {
            return Result.Fail(getUserEmailResult.Errors);
        }

        var isEmailOccupiedResult =
            await _usersQueriesRepository.IsEmailOccupiedAsync(getUserEmailResult.ValueOrDefault);
        if (isEmailOccupiedResult.IsFailed)
        {
            return Result.Fail(isEmailOccupiedResult.Errors);
        }

        if (isEmailOccupiedResult.ValueOrDefault)
        {
            return RegisterErrors.EmailIsOccupied(getUserEmailResult.ValueOrDefault);
        }
        
        return Result.Ok();
    }
}