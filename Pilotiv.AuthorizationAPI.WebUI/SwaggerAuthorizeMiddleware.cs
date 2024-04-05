using System.Net;
using System.Text;

namespace Pilotiv.AuthorizationAPI.WebUI;

/// <summary>
/// Компонент Middleware авторизация Swagger.
/// </summary>
public class SwaggerAuthorizeMiddleware : IMiddleware
{
    /// <summary>
    /// Обработка пайплайна запроса.
    /// </summary>
    /// <param name="context">Контекст Http.</param>
    /// <param name="next">Делегат запроса.</param>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader is not null && authHeader.StartsWith("Basic "))
            {
                // Get the encoded username and password
                var encodedUsernamePassword =
                    authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1].Trim();

                // Decode from Base64 to string
                var decodedUsernamePassword =
                    Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

                // Split username and password
                var username = decodedUsernamePassword.Split(':', 2)[0];
                var password = decodedUsernamePassword.Split(':', 2)[1];

                // Check if login is correct
                if (IsAuthorized(username, password))
                {
                    await next.Invoke(context);
                    return;
                }
            }

            // Return authentication type (causes browser to show login dialog)
            context.Response.Headers["WWW-Authenticate"] = "Basic";

            // Return unauthorized
            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
        }
        else
        {
            await next.Invoke(context);
        }
    }

    private static bool IsAuthorized(string username, string password)
    {
        // Check that username and password are correct
        return username.Equals("admin", StringComparison.InvariantCultureIgnoreCase)
               && password.Equals("admin");
    }

    private static bool IsLocalRequest(HttpContext context)
    {
        // Handle running using the Microsoft.AspNetCore.TestHost and the site being run entirely locally in memory without an actual TCP/IP connection
        if (context.Connection.RemoteIpAddress is null && context.Connection.LocalIpAddress is null)
        {
            return true;
        }

        if (context.Connection.RemoteIpAddress?.Equals(context.Connection.LocalIpAddress) is true)
        {
            return true;
        }

        if (context.Connection.RemoteIpAddress is not null && IPAddress.IsLoopback(context.Connection.RemoteIpAddress))
        {
            return true;
        }

        return false;
    }
}