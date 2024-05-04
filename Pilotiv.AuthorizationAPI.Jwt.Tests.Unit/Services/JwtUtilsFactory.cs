using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pilotiv.AuthorizationAPI.Jwt.ConfigurationOptions;

namespace Pilotiv.AuthorizationAPI.JwtProvider.Tests.Unit.Services;

public static class JwtUtilsFactory
{
    /// <summary>
    /// Создание сервиса работы с JWT.
    /// </summary>
    /// <param name="path">Путь к файлу конфигурации.</param>
    /// <returns>Сервис работы с JWT.</returns>
    public static Jwt.Services.JwtProvider Create(string path)
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
        
        return new Jwt.Services.JwtProvider(authenticationKeysOption);
    }
}