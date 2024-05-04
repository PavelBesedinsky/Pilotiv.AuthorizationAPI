using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Pilotiv.AuthorizationAPI.Jwt.Entities;

namespace Pilotiv.AuthorizationAPI.JwtProvider.Tests.Unit.Services;

/// <summary>
/// Тесты валидации токена доступа.
/// </summary>
public class ValidateAccessTokenTests
{
    /// <summary>
    /// Тест валидации токена доступа с нагрузкой.
    /// </summary>
    [Fact]
    public Task ValidateAccessToken_WhenPublicKeyExists_WithCustomPayload_Test()
    {
        var accessToken = GenerateAccessToken(new Dictionary<string, string> {{"id", "id"}});
        Assert.NotNull(accessToken);
        Assert.NotEmpty(accessToken);

        var jwtUtils = JwtUtilsFactory.Create("appsettings.test.pubkey.json");
        var result = jwtUtils.ValidateAccessToken(accessToken).ToList();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.True(result.Any(claim => claim.Type == "id"));

        return Task.CompletedTask;
    }

    /// <summary>
    /// Тест валидации токена доступа.
    /// </summary>
    [Fact]
    public Task ValidateAccessToken_WhenPublicKeyExists_WithoutCustomPayload_Test()
    {
        var accessToken = GenerateAccessToken(new());
        Assert.NotNull(accessToken);
        Assert.NotEmpty(accessToken);

        var jwtUtils = JwtUtilsFactory.Create("appsettings.test.pubkey.json");
        var result = jwtUtils.ValidateAccessToken(accessToken);
        Assert.NotNull(result);
        Assert.NotEmpty(result);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Тест валидации токена доступа при отсутствующем публичном ключе.
    /// </summary>
    [Fact]
    public Task ValidateAccessToken_WithFakePublicKey_Test()
    {
        var accessToken = GenerateAccessToken(new());
        Assert.NotNull(accessToken);
        Assert.NotEmpty(accessToken);

        var jwtUtils = JwtUtilsFactory.Create("appsettings.test.fake_pubkey.json");
        Assert.Throws<CryptographicException>(() => jwtUtils.ValidateAccessToken(accessToken));

        return Task.CompletedTask;
    }

    /// <summary>
    /// Тест валидации токена доступа при отсутствующем публичном ключе.
    /// </summary>
    [Fact]
    public Task ValidateAccessToken_WithoutPublicKey_Test()
    {
        var accessToken = GenerateAccessToken(new());
        Assert.NotNull(accessToken);
        Assert.NotEmpty(accessToken);

        var jwtUtils = JwtUtilsFactory.Create("appsettings.test.privatekey.json");
        Assert.Throws<ArgumentException>(() => jwtUtils.ValidateAccessToken(accessToken));

        return Task.CompletedTask;
    }

    /// <summary>
    /// Создание токена доступа.
    /// </summary>
    /// <returns>Токен доступа.</returns>
    private string GenerateAccessToken(Dictionary<string, string> payload)
    {
        var jwtUtils = JwtUtilsFactory.Create("appsettings.test.privatekey.json");

        var accessToken = jwtUtils.GenerateAccessToken(new AccessTokenConfiguration
        {
            ExpiringHours = 1,
            Payload = payload
        });

        return accessToken;
    }
}