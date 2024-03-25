using NSwag.Generation.AspNetCore;
using OpenApiContact = NSwag.OpenApiContact;
using OpenApiInfo = NSwag.OpenApiInfo;

namespace Pilotiv.AuthorizationAPI.WebUI.Settings;

/// <summary>
/// Настройка данных и описания страницы Swagger.
/// </summary>
public static class OpenApiSettings
{
    /// <summary>
    /// Получение настроек.
    /// </summary>
    public static void OpenApiDocument(AspNetCoreOpenApiDocumentGeneratorSettings options)
    {
        options.PostProcess = PostProcess;
    }

    /// <summary>
    /// Обработка.
    /// </summary>
    /// <param name="document"><see cref="NSwag.OpenApiDocument "/>.</param>
    private static void PostProcess(NSwag.OpenApiDocument document)
    {
        document.Info = new OpenApiInfo
        {
            Title = "Pilotiv.Authorization",
            Description = "API авторизации.",
            TermsOfService = null,
            Contact = new OpenApiContact
            {
                Name = "tg",
                Url = "https://t.me/pilotiv"
            },
            Version = "0.0.1"
        };
    }
}