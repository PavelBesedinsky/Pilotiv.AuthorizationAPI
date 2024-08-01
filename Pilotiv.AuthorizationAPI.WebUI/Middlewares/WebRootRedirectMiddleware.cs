using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Pilotiv.AuthorizationAPI.WebUI.Middlewares;

/// <summary>
/// Компонент Middleware переадресации к WebRoot.
/// </summary>
public class WebRootRedirectMiddleware : IMiddleware
{
    /// <summary>
    /// Обработка пайплайна запроса.
    /// </summary>
    /// <param name="context">Контекст Http.</param>
    /// <param name="next">Делегат запроса.</param>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next(context);

        var requestPath = context.Request.Path.Value ?? string.Empty;
        if (context.Response.StatusCode is StatusCodes.Status404NotFound && !Path.HasExtension(requestPath) &&
            !requestPath.StartsWith("/api"))
        {
            context.Request.Path = "/index.html";
            await next(context);
        }
    }
}