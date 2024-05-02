using FluentResults;
using MediatR;
using Microsoft.Extensions.Options;
using Pilotiv.Authentication.ConfigurationOptions;
using Pilotiv.Authentication.Entities;
using Pilotiv.Authentication.Services;
using Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.Authorize.Dtos;
using Pilotiv.AuthorizationAPI.Application.Shared.Dtos;
using Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Commands;
using Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Repositories.Queries;
using Pilotiv.AuthorizationAPI.Domain.Models.Users;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

namespace Pilotiv.AuthorizationAPI.Application.Requests.Auth.Commands.Authorize;

/// <summary>
/// Обработчик команды авторизации.
/// </summary>
public class AuthorizeCommandHandler : IRequestHandler<AuthorizeCommand, Result<AuthorizeCommandResponse>>
{
    private readonly IUsersCommandsRepository _usersCommandsRepository;
    private readonly IUsersQueriesRepository _usersQueriesRepository;
    private readonly JwtUtils _jwtUtils;

    /// <summary>
    /// Создание обработчика команды авторизации.
    /// </summary>
    /// <param name="authenticationKeysOption">Настройки сервиса авторизации.</param>
    /// <param name="usersCommandsRepository">Интерфейс репозитория команд пользователей.</param>
    /// <param name="usersQueriesRepository">Интерфейс репозитория запросов пользователей.</param>
    public AuthorizeCommandHandler(IOptionsMonitor<AuthenticationKeysOption> authenticationKeysOption,
        IUsersCommandsRepository usersCommandsRepository, IUsersQueriesRepository usersQueriesRepository)
    {
        _usersCommandsRepository = usersCommandsRepository;
        _usersQueriesRepository = usersQueriesRepository;
        _jwtUtils = new JwtUtils(authenticationKeysOption);
    }

    /// <summary>
    /// Обработка команды авторизации.
    /// </summary>
    /// <param name="request">Команда авторизации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объекта переноса данных ответа команды авторизации.</returns>
    public async Task<Result<AuthorizeCommandResponse>> Handle(AuthorizeCommand request,
        CancellationToken cancellationToken = default)
    {
        List<IError> errors = new();

        var getUserLoginResult = UserLogin.Create(request.Login);
        if (getUserLoginResult.IsFailed)
        {
            errors.AddRange(getUserLoginResult.Errors);
        }

        var getUserPasswordResult = UserPasswordHash.Create(request.Password);
        if (getUserPasswordResult.IsFailed)
        {
            errors.AddRange(getUserPasswordResult.Errors);
        }

        if (errors.Any())
        {
            return Result.Fail(errors);
        }

        var userLogin = getUserLoginResult.ValueOrDefault;
        var userPassword = getUserPasswordResult.ValueOrDefault;

        var getUserResult = await _usersQueriesRepository.GetUserByLoginPasswordAsync(userLogin, userPassword);
        if (getUserResult.IsFailed)
        {
            return getUserResult.ToResult();
        }

        var user = getUserResult.ValueOrDefault;

        var accessToken = GenerateAccessTokenForUser(user);
        var createRefreshTokenResult = CreateRefreshToken(request.Ip);
        if (createRefreshTokenResult.IsFailed)
        {
            return createRefreshTokenResult.ToResult();
        }

        var refreshToken = createRefreshTokenResult.ValueOrDefault;
        var addRefreshTokenResult = user.AddRefreshToken(refreshToken);
        if (addRefreshTokenResult.IsFailed)
        {
            return addRefreshTokenResult;
        }

        await _usersCommandsRepository.CommitChangesAsync(user, cancellationToken);
        
        return new AuthorizeCommandResponse
        {
            AccessToken = accessToken,
            RefreshToken = new RefreshTokenPayload
            {
                Token = refreshToken.Id.Value,
                Expires = refreshToken.ExpirationDate
            }
        };
    }

    /// <summary>
    /// Создание токена доступа для пользователя.
    /// </summary>
    /// <param name="user">Пользователь.</param>
    /// <returns>Токен доступа.</returns>
    private string GenerateAccessTokenForUser(User user)
    {
        return _jwtUtils.GenerateAccessToken(new()
        {
            ExpiringHours = 1,
            Payload = new Dictionary<string, string>
            {
                {"email", user.Email.Value}
            }
        });
    }
    
    /// <summary>
    /// Создание токена обновления.
    /// </summary>
    /// <param name="ip">IP-адрес пользователя, с которого выполняется авторизация.</param>
    /// <returns>Токен обновления.</returns>
    private Result<Domain.Models.Users.Entities.RefreshToken> CreateRefreshToken(string? ip)
    {
        var refreshToken =_jwtUtils.GenerateRefreshToken(new()
        {
            ExpiringHours = 720,
            Ip = ip
        });
        
        var factory = new RefreshTokenFactory(new(refreshToken.Token)
        {
            ExpirationDate = refreshToken.Expires,
            CreatedDate = refreshToken.Created,
            CreatedByIp = refreshToken.CreatedByIp
        });

        return factory.Create();
    }
}