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
internal class JwtProvider : IJwtProvider
{
    private readonly AuthenticationKeysOption _options;

    /// <summary>
    /// Сервис работы с JWT.
    /// </summary>
    /// <param name="options">Опция для генерации и валидации ключей доступа.</param>
    internal JwtProvider(IOptionsMonitor<AuthenticationKeysOption> options)
    {
        _options = options.CurrentValue;
    }

    /// <inheritdoc />
    public string GenerateAccessToken(AccessTokenConfiguration configuration)
    {
        var tokenDescriptor = GetTokenDescriptor(configuration, _options);

        JwtSecurityTokenHandler tokenHandler = new();
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

        var tokenValidationParameters = GetTokenValidationParameters(publicKey);

        try
        {
            JwtSecurityTokenHandler tokenHandler = new();
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
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        var refreshToken = new RefreshToken(Convert.ToBase64String(randomNumber),
            configuration.Expires, DateTime.UtcNow, configuration.Ip ?? string.Empty);

        return refreshToken;
    }

    /// <summary>
    /// Получение дескриптора токена.
    /// </summary>
    /// <param name="accessTokenConfiguration">Конфигурация токена.</param>
    /// <param name="options">Опция для генерации и валидации ключей доступа.</param>
    /// <returns>Дескриптор токена.</returns>
    private static SecurityTokenDescriptor GetTokenDescriptor(AccessTokenConfiguration accessTokenConfiguration,
        AuthenticationKeysOption options)
    {
        var privateKey = options.PrivateKey;
        if (string.IsNullOrWhiteSpace(privateKey))
        {
            throw new ArgumentException(string.Format(Resources.PrivateKeyNotFoundError,
                nameof(AuthenticationKeysOption.AuthenticationKeys)));
        }

        SigningAudienceCertificate signingAudienceCertificate = new();
        var audienceSigningKey = signingAudienceCertificate.GetAudienceSigningKey(privateKey);

        var claims = accessTokenConfiguration.Claims.Select(item => new Claim(item.Key, item.Value));

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            NotBefore = accessTokenConfiguration.NotBefore,
            Expires = accessTokenConfiguration.Expires,
            Issuer = options.Issuer,
            Audience = options.Audience,
            SigningCredentials = audienceSigningKey
        };

        return tokenDescriptor;
    }

    /// <summary>
    /// Получение параметров валидации токена.
    /// </summary>
    /// <param name="publicKey">Публичный ключ валидации токена.</param>
    /// <param name="issuer">Создатель ключа.</param>
    /// <param name="audience">Потребитель ключа.</param>
    /// <returns>Параметры валидации токена.</returns>
    public static TokenValidationParameters GetTokenValidationParameters(string publicKey, string? issuer = null,
        string? audience = null)
    {
        SigningIssuerCertificate issuerSigningCertificate = new();
        var issuerSigningKey = issuerSigningCertificate.GetIssuerSingingKey(publicKey);

        return new TokenValidationParameters
        {
            ValidateAudience = audience is not null,
            ValidateIssuer = issuer is not null,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = issuerSigningKey,
            ValidIssuer = issuer,
            ValidAudience = audience,
            LifetimeValidator = TokenValidators.LifetimeValidator
        };
    }
}