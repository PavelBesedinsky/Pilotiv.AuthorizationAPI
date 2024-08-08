using Pilotiv.AuthorizationAPI.Jwt.Entities;

namespace Pilotiv.AuthorizationAPI.Jwt.Tests.Unit.Services;

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
        var jwtUtils = JwtProviderFactory.Create("appsettings.test.both.json");
        var refreshToken = jwtUtils.GenerateRefreshToken(new RefreshTokenConfiguration());
        Assert.NotNull(refreshToken);
        Assert.NotEmpty(refreshToken.Token);
    }
}