namespace Pilotiv.AuthorizationAPI.WebUI.Extensions;

/// <summary>
/// Расширения для <see cref="IHostEnvironment"/>.
/// </summary>
public static class HostEnvironmentExtensions
{
    private const string Container = nameof(Container);
    
    /// <summary>
    /// Получение признака работы в окружении <see cref="Container"/>.
    /// </summary>
    /// <param name="hostEnvironment">Провайдер окружения хоста.</param>
    public static bool IsContainer(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsEnvironment(Container);
    }
}