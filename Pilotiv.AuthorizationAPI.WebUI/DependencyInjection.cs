using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Pilotiv.Authentication.Certificates;
using Pilotiv.Authentication.ConfigurationOptions;
using Pilotiv.AuthorizationAPI.WebUI.Middlewares;
using Pilotiv.AuthorizationAPI.WebUI.Settings;

namespace Pilotiv.AuthorizationAPI.WebUI;

/// <summary>
/// Класс определения зависимостей слоя "Презентация"
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавление зависимостей слоя "Приложение"
    /// </summary>
    /// <param name="builder">Констуктор приложения.</param>
    /// <returns>Конструктор приложения.</returns>
    public static IHostApplicationBuilder AddPresentation(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddAuthentication(builder.Configuration);
        services.AddControllers()
            .ConfigureApiBehaviorOptions(opt => { opt.SuppressMapClientErrors = true; });
        services.AddEndpointsApiExplorer();

        services.AddOpenApiDocument(OpenApiSettings.OpenApiDocument);

        services.AddScoped<WebRootRedirectMiddleware>();
        services.AddScoped<SwaggerAuthorizeMiddleware>();

        return builder;
    }

    /// <summary>
    /// Добавление зависимостей слоя инфраструктуры
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <param name="configuration">Конфигурация</param>
    /// <returns>Коллекция сервисов</returns>
    /// <exception cref="ArgumentNullException">Исключение выбрасывается при отсутствующем PublicKey в конфигурации</exception>
    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
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
                    LifetimeValidator = LifetimeValidator
                };
            });
        
        return services;
    }

    private static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken,
        TokenValidationParameters validationParameters)
    {
        return expires != null && expires > DateTime.UtcNow;
    }
}