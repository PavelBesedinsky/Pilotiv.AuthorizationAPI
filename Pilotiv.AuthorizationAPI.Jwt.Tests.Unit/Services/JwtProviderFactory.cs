using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pilotiv.AuthorizationAPI.Jwt.Abstractions;
using Pilotiv.AuthorizationAPI.Jwt.ConfigurationOptions;
using Pilotiv.AuthorizationAPI.Jwt.Factories;
using Pilotiv.AuthorizationAPI.Jwt.Services;

namespace Pilotiv.AuthorizationAPI.Jwt.Tests.Unit.Services;

/// <summary>
/// Фабрика сервиса работы с JWT.
/// </summary>
public static class JwtProviderFactory
{
    /// <summary>
    /// Создание сервиса работы с JWT.
    /// </summary>
    /// <param name="path">Путь к файлу конфигурации.</param>
    /// <returns>Сервис работы с JWT.</returns>
    public static IJwtProvider Create(string path)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddOptions();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(path)
            .AddEnvironmentVariables()
            .Build();

        serviceCollection.Configure<AuthenticationKeysOption>(
            configuration.GetSection(AuthenticationKeysOption.AuthenticationKeys));

        var services = serviceCollection.BuildServiceProvider();

        var authenticationKeysOption = services.GetService<IOptionsMonitor<AuthenticationKeysOption>>();
        Assert.NotNull(authenticationKeysOption);

        return Factories.JwtProviderFactory.CreateJwtProvider(authenticationKeysOption);
    }
}