using Pilotiv.AuthorizationAPI.Application;
using Pilotiv.AuthorizationAPI.Infrastructure;
using Pilotiv.AuthorizationAPI.WebUI;
using Pilotiv.AuthorizationAPI.WebUI.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseHostExtensions();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddPresentation();
builder.Services.ConfigureOptions(builder);

var app = builder.Build();

var logger = app.Services.GetService<ILogger<Program>>();

logger?.LogInformation("Environment: {EnvironmentName}", builder.Environment.EnvironmentName);

app.UseWebApplicationExtension();

app.Run();