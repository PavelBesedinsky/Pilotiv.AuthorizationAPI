namespace Pilotiv.AuthorizationAPI.WebUI.Settings;

/// <summary>
/// Настройки "ReDoc".
/// </summary>
public static class ReDocSettings
{
    /// <summary>
    /// Применить настройки для "ReDoc".
    /// </summary>
    /// <param name="settings"></param>
    public static void Apply(NSwag.AspNetCore.ReDocSettings settings)
    {
        settings.Path = "/redoc";
    }
}