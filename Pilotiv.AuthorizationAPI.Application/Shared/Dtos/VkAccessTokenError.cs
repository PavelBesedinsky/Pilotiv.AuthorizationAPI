namespace Pilotiv.AuthorizationAPI.Application.Shared.Dtos;

/// <summary>
/// Объект переноса данных ошибки получения токен доступа.
/// </summary>
public class VkAccessTokenError
{
    /// <summary>
    /// Тип ошибки.
    /// </summary>
    public string? Error { get; init; }

    /// <summary>
    /// Описание ошибки.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public string? Error_Description { get; init; }
}