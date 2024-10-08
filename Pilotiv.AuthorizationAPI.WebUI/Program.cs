using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pilotiv.AuthorizationAPI.Application;
using Pilotiv.AuthorizationAPI.Infrastructure;
using Pilotiv.AuthorizationAPI.Jwt;
using Pilotiv.AuthorizationAPI.WebUI;
using Pilotiv.AuthorizationAPI.WebUI.Extensions;

var exePath = Process.GetCurrentProcess().MainModule?.FileName;
var rootPath = Path.GetDirectoryName(exePath) ?? string.Empty;
var webRootPath = Path.Combine(rootPath, "wwwroot");

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = webRootPath
});

builder.Host.UseHostExtensions();

builder.AddJwtProviderAuthentication();
builder.AddApplication();
builder.AddInfrastructure();
builder.AddPresentation();
builder.Services.ConfigureOptions(builder);

var app = builder.Build();

var logger = app.Services.GetService<ILogger<Program>>();

logger?.LogInformation("Environment: {EnvironmentName}", builder.Environment.EnvironmentName);

app.ConfigureWebApplication();

app.Run();