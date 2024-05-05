using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Pilotiv.AuthorizationAPI.Jwt.Abstractions;
using Pilotiv.AuthorizationAPI.Jwt.Certificates;
using Pilotiv.AuthorizationAPI.Jwt.ConfigurationOptions;
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
        var publicKey = configuration.GetSection(AuthenticationKeysOption.AuthenticationKeys)
            .Get<AuthenticationKeysOption>()?.PublicKey;
        if (string.IsNullOrWhiteSpace(publicKey))
        {
            throw new ArgumentException(nameof(AuthenticationKeysOption.PublicKey));
        }

        var issuerSigningCertificate = new SigningIssuerCertificate();
        var issuerSigningKey = issuerSigningCertificate.GetIssuerSingingKey(publicKey);

        services
            .AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = issuerSigningKey,
                    LifetimeValidator = Validators.LifetimeValidator
                };
            });

        return builder;
    }
}