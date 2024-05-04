﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pilotiv.AuthorizationAPI.Jwt.Entities;

namespace Pilotiv.AuthorizationAPI.JwtProvider.Tests.Unit.Services;

/// <summary>
/// Тесты создания токена доступа.
/// </summary>
public class GenerateAccessTokenTests
{
    /// <summary>
    /// Тест создания токена доступа, с файлом конфигурации содержащим публичный и приватный ключи.
    /// </summary>
    [Fact]
    public Task GenerateAccessToken_WhenConfigContainsBothKeys_Test()
    {
        var jwtUtils = JwtUtilsFactory.Create("appsettings.test.both.json");

        var accessToken = jwtUtils.GenerateAccessToken(new AccessTokenConfiguration
        {
            ExpiringHours = 1,
            Payload = new Dictionary<string, string> {{"id", "id"}}
        });
        Assert.NotNull(accessToken);
        Assert.NotEmpty(accessToken);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Тест создания токена доступа, с файлом конфигурации содержащим публичный ключ.
    /// </summary>
    [Fact]
    public Task GenerateAccessToken_WhenConfigContainsPublicKey_Test()
    {
        var jwtUtils = JwtUtilsFactory.Create("appsettings.test.pubkey.json");

        Assert.Throws<ArgumentException>(() => jwtUtils.GenerateAccessToken(new AccessTokenConfiguration
        {
            ExpiringHours = 1,
            Payload = new Dictionary<string, string> {{"id", "id"}}
        }));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Тест создания токена доступа, с файлом конфигурации содержащим приватный ключ.
    /// </summary>
    [Fact]
    public Task GenerateAccessToken_WhenConfigContainsPrivateKey_Test()
    {
        var jwtUtils = JwtUtilsFactory.Create("appsettings.test.privatekey.json");

        var accessToken = jwtUtils.GenerateAccessToken(new AccessTokenConfiguration
        {
            ExpiringHours = 1,
            Payload = new Dictionary<string, string> {{"id", "id"}}
        });
        Assert.NotNull(accessToken);
        Assert.NotEmpty(accessToken);

        return Task.CompletedTask;
    }
}