using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pilotiv.AuthorizationAPI.Jwt.Abstractions;
using Pilotiv.AuthorizationAPI.Jwt.Certificates;
using Pilotiv.AuthorizationAPI.Jwt.ConfigurationOptions;
using Pilotiv.AuthorizationAPI.Jwt.Entities;
using Pilotiv.AuthorizationAPI.Jwt.Properties;

namespace Pilotiv.AuthorizationAPI.Jwt.Services;

/// <summary>
/// Сервис работы с токенами.
/// </summary>
public class JwtProvider : IJwtProvider
{
    private readonly AuthenticationKeysOption _options;

    /// <summary>
    /// Сервис работы с JWT.
    /// </summary>
    /// <param name="options">Опция для генерации и валидации ключей доступа.</param>
    public JwtProvider(IOptionsMonitor<AuthenticationKeysOption> options)
    {
        _options = options.CurrentValue;
    }

    /// <inheritdoc />
    public string GenerateAccessToken(AccessTokenConfiguration configuration)
    {
        var tokenDescriptor = GetTokenDescriptor(configuration, _options);
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    /// <inheritdoc />
    public IEnumerable<Claim> ValidateAccessToken(string token)
    {
        var publicKey = _options.PublicKey;
        if (string.IsNullOrWhiteSpace(publicKey))
        {
            throw new ArgumentException(string.Format(Resources.PublicKeyNotFoundError,
                nameof(AuthenticationKeysOption.AuthenticationKeys)));
        }

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenValidationParameters = GetTokenValidationParameters(publicKey);

        try
        {
            tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken)
            {
                return Enumerable.Empty<Claim>();
            }
            
            return jwtSecurityToken.Claims;
        }
        catch
        {
            return Enumerable.Empty<Claim>();
        }
    }

    /// <inheritdoc />
    public RefreshToken GenerateRefreshToken(RefreshTokenConfiguration configuration)
    {
        var currentDate = DateTime.UtcNow;

        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        var refreshToken = new RefreshToken(Convert.ToBase64String(randomNumber),
            currentDate.AddHours(configuration.ExpiringHours), currentDate, configuration.Ip ?? string.Empty);

        return refreshToken;
    }

    /// <summary>
    /// Получение дескриптера токена.
    /// </summary>
    /// <param name="configuration">Конфигурация токена.</param>
    /// <param name="options">Опция для генерации и валидации ключей доступа.</param>
    /// <returns>Дескриптер токена.</returns>
    private static SecurityTokenDescriptor GetTokenDescriptor(AccessTokenConfiguration configuration,
        AuthenticationKeysOption options)
    {
        var privateKey = options.PrivateKey;
        if (string.IsNullOrWhiteSpace(privateKey))
        {
            throw new ArgumentException(string.Format(Resources.PrivateKeyNotFoundError,
                nameof(AuthenticationKeysOption.AuthenticationKeys)));
        }

        var signingAudienceCertificate = new SigningAudienceCertificate();
        var audienceSigningKey = signingAudienceCertificate.GetAudienceSigningKey(privateKey);

        var claims = configuration.Payload.Select(item => new Claim(item.Key, item.Value));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(configuration.ExpiringHours),
            SigningCredentials = audienceSigningKey
        };

        return tokenDescriptor;
    }

    /// <summary>
    /// Получение параметров валидации токена.
    /// </summary>
    /// <param name="publicKey">Публичный ключ валидации токена.</param>
    /// <returns>Параметры валидации токена.</returns>
    private static TokenValidationParameters GetTokenValidationParameters(string publicKey)
    {
        var issuerSigningCertificate = new SigningIssuerCertificate();
        var issuerSigningKey = issuerSigningCertificate.GetIssuerSingingKey(publicKey);

        return new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = issuerSigningKey,
            LifetimeValidator = Validators.LifetimeValidator
        };
    }
}