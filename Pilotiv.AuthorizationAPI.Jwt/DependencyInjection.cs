using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pilotiv.AuthorizationAPI.Jwt.Abstractions;
using Pilotiv.AuthorizationAPI.Jwt.ConfigurationOptions;
using Pilotiv.AuthorizationAPI.Jwt.Extensions;
using Pilotiv.AuthorizationAPI.Jwt.Services;

namespace Pilotiv.AuthorizationAPI.Jwt;

/// <summary>
/// Подключение зависимостей.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавление зависимостей слоя инфраструктуры
    /// </summary>
    /// <exception cref="ArgumentNullException">Исключение выбрасывается при отсутствующем PublicKey в конфигурации</exception>
    public static IHostApplicationBuilder AddJwtProviderAuthentication(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        
        // Подключение сервиса работы с токенами.
        services.AddSingleton<IJwtProvider, JwtProvider>();

        // Подключение аутентификации на основе JWT.
        var authenticationKeys = configuration.GetSection(AuthenticationKeysOption.AuthenticationKeys)
            .Get<AuthenticationKeysOption>();
        var publicKey = authenticationKeys?.PublicKey;
        if (string.IsNullOrWhiteSpace(publicKey))
        {
            throw new ArgumentException(nameof(AuthenticationKeysOption.PublicKey));
        }

        var issuer = authenticationKeys?.Issuer;
        var audience = authenticationKeys?.Audience;
        
        services
            .AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = JwtProvider.GetTokenValidationParameters(publicKey, issuer, audience);
            });
        services.AddAuthorization();
        
        services.AddAuthenticationKeysOption(configuration);
        
        return builder;
    }
}