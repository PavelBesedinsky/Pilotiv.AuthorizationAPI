using FluentResults;
using MediatR;
using Microsoft.Extensions.Options;
using Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.ObtainVkToken.Dtos;
using Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.ObtainVkToken.Errors;
using Pilotiv.AuthorizationAPI.Application.Shared.Dtos;
using Pilotiv.AuthorizationAPI.Application.Shared.Factories.Users;
using Pilotiv.AuthorizationAPI.Application.Shared.Factories.Users.Dtos;
using Pilotiv.AuthorizationAPI.Application.Shared.Options;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Commands;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Queries;
using Pilotiv.AuthorizationAPI.Application.Shared.Services;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.ObtainVkToken;

/// <summary>
/// Обработчик команды получения токена VK.
/// </summary>
public class ObtainVkTokenCommandHandler : IRequestHandler<ObtainVkTokenCommand, Result<ObtainVkTokenCommandResponse>>
{
    private readonly IOAuthVkProvider _oAuthVkProvider;
    private readonly IUsersCommandsRepository _usersCommandsRepository;
    private readonly IUsersQueriesRepository _usersQueriesRepository;
    private readonly OAuthVkCredentialsOptions _oAuthVkCredentials;

    /// <summary>
    /// Создание обработчика команды получения токена VK.
    /// </summary>
    /// <param name="oAuthVkCredentialsOptionsMonitor">Настройки для Authorization Code Flow для получения Access token пользователя.</param>
    /// <param name="oAuthVkProvider">Провайдер взаимодействия с сервисом авторизации VK.</param>
    /// <param name="usersCommandsRepository">Интерфейс репозитория команд пользователей.</param>
    /// <param name="usersQueriesRepository">Интерфейс репозитория запросов пользователей.</param>
    public ObtainVkTokenCommandHandler(IOptionsMonitor<OAuthVkCredentialsOptions> oAuthVkCredentialsOptionsMonitor,
        IOAuthVkProvider oAuthVkProvider, IUsersCommandsRepository usersCommandsRepository,
        IUsersQueriesRepository usersQueriesRepository)
    {
        _oAuthVkProvider = oAuthVkProvider;
        _usersCommandsRepository = usersCommandsRepository;
        _usersQueriesRepository = usersQueriesRepository;
        _oAuthVkCredentials = oAuthVkCredentialsOptionsMonitor.CurrentValue;
    }

    /// <summary>
    /// Обработка команды получения токена VK.
    /// </summary>
    /// <param name="request">Команда получения токена VK.</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект переноса данных результата команды получения токена VK.</returns>
    public async Task<Result<ObtainVkTokenCommandResponse>> Handle(ObtainVkTokenCommand request,
        CancellationToken cancellationToken = default)
    {
        var code = request.Code;
        var isVkCredentialsValidResult = ValidateVkCredentials(code);
        if (isVkCredentialsValidResult.IsFailed)
        {
            return isVkCredentialsValidResult;
        }

        var getAccessTokenPayloadResult = await _oAuthVkProvider.GetAccessTokenAsync(_oAuthVkCredentials.ClientId!,
            _oAuthVkCredentials.ClientSecret!, _oAuthVkCredentials.RedirectUri!, code, cancellationToken);
        if (getAccessTokenPayloadResult.IsFailed)
        {
            return getAccessTokenPayloadResult.ToResult();
        }

        var payload = getAccessTokenPayloadResult.Value;

        var getVkInternalIdResult = VkInternalUserId.Create(payload.User_Id.ToString());
        if (getVkInternalIdResult.IsFailed)
        {
            return getVkInternalIdResult.ToResult();
        }

        var vkInternalId = getVkInternalIdResult.ValueOrDefault;

        var isUserExistsResult = await _usersQueriesRepository.IsVkUserExistsAsync(vkInternalId);
        if (isUserExistsResult.IsFailed)
        {
            return isUserExistsResult.ToResult();
        }

        var isUserExists = isUserExistsResult.ValueOrDefault;

        Result<User> getUserResult;

        if (!isUserExists)
        {
            var getCheckingEmail = UserEmail.Create(payload.Email ?? string.Empty);
            if (getCheckingEmail.IsFailed)
            {
                return getCheckingEmail.ToResult();
            }

            var checkingEmail = getCheckingEmail.ValueOrDefault;

            var isEmailOccupiedResult = await _usersQueriesRepository.IsEmailOccupiedAsync(checkingEmail);
            if (isEmailOccupiedResult.IsFailed)
            {
                return isEmailOccupiedResult.ToResult();
            }

            var isEmailOccupied = isEmailOccupiedResult.ValueOrDefault;
            if (isEmailOccupied)
            {
                return ObtainVkTokenErrors.EmailIsOccupied(checkingEmail);
            }

            getUserResult = CreateUser(payload);
        }
        else
        {
            getUserResult = await UpdateUserAsync(vkInternalId);
        }

        if (getUserResult.IsFailed)
        {
            return getUserResult.ToResult();
        }

        var user = getUserResult.ValueOrDefault;

        await _usersCommandsRepository.CommitChangesAsync(user, cancellationToken);

        return new ObtainVkTokenCommandResponse
        {
            AccessToken = getAccessTokenPayloadResult.Value.Access_Token,
            IsNew = !isUserExists
        };
    }

    /// <summary>
    /// Создание пользователя.
    /// </summary>
    /// <param name="payload">Объект переноса данных токена доступа VK.</param>
    /// <returns>Пользователь.</returns>
    private static Result<User> CreateUser(VkAccessTokenPayload payload)
    {
        var usersFabric = new UserFactory(new UsersFactoryUserPayload
        {
            Email = payload.Email,
            RegistrationDate = DateTime.UtcNow,
            AuthorizationDate = DateTime.UtcNow,
            VkUser = new UsersFactoryVkUserPayload
            {
                InternalUserId = payload.User_Id.ToString()
            }
        });

        return usersFabric.Create();
    }

    /// <summary>
    /// Обновление пользователя.
    /// </summary>
    /// <param name="internalUserId">Внутренний идентификатор пользователя в VK.</param>
    /// <returns>Пользователь.</returns>
    private async Task<Result<User>> UpdateUserAsync(VkInternalUserId internalUserId)
    {
        var getUserResult = await _usersQueriesRepository.GetUserByVkInternalIdAsync(internalUserId);
        if (getUserResult.IsFailed)
        {
            return getUserResult.ToResult();
        }

        var user = getUserResult.ValueOrDefault;

        var createAuthorizationDateResult = UserAuthorizationDate.Create(DateTime.UtcNow);
        if (createAuthorizationDateResult.IsFailed)
        {
            return createAuthorizationDateResult.ToResult();
        }

        var updateAuthorizationDateResult = user.UpdateAuthorizationDate(createAuthorizationDateResult.ValueOrDefault);
        if (updateAuthorizationDateResult.IsFailed)
        {
            return updateAuthorizationDateResult;
        }

        return user;
    }

    /// <summary>
    /// Валидация настроек для Authorization Code Flow для получения Access token пользователя.
    /// </summary>
    /// <param name="code">Код авторизации.</param>
    private Result ValidateVkCredentials(string code)
    {
        List<Error> errors = new();

        if (string.IsNullOrWhiteSpace(code))
        {
            errors.Add(ObtainVkTokenErrors.CodeIsNullOrEmpty());
        }

        if (string.IsNullOrWhiteSpace(_oAuthVkCredentials.ClientId))
        {
            errors.Add(ObtainVkTokenErrors.ClientIdIsNullOrEmpty());
        }

        if (string.IsNullOrWhiteSpace(_oAuthVkCredentials.ClientSecret))
        {
            errors.Add(ObtainVkTokenErrors.ClientSecretIsNullOrEmpty());
        }

        if (string.IsNullOrWhiteSpace(_oAuthVkCredentials.RedirectUri))
        {
            errors.Add(ObtainVkTokenErrors.RedirectUriIsNullOrEmpty());
        }

        if (errors.Any())
        {
            return errors;
        }

        return Result.Ok();
    }
}