﻿using Pilotiv.AuthorizationAPI.Jwt.Entities;

namespace Pilotiv.AuthorizationAPI.JwtProvider.Tests.Unit.Services;

/// <summary>
/// Тесты создания токена обновления.
/// </summary>
public class GenerateRefreshTokenTests
{
    /// <summary>
    /// Тест создания токена обновления.
    /// </summary>
    [Fact]
    public void GenerateRefreshTokenTest()
    {
        var jwtUtils = JwtUtilsFactory.Create("appsettings.test.both.json");
        var refreshToken = jwtUtils.GenerateRefreshToken(new RefreshTokenConfiguration());
        Assert.NotNull(refreshToken);
        Assert.NotEmpty(refreshToken.Token);
    }
}