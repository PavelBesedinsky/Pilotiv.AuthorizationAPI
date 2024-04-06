namespace Pilotiv.AuthorizationAPI.Application.Shared.Dtos;

/// <summary>
/// Объект переноса данных токена доступа VK.
/// </summary>
public class VkAccessToken
{
    /// <summary>
    /// Токен доступа.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public string? Access_Token { get; init; }

    /// <summary>
    /// Время, через которое токен дроступа станет невалидным.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public int Expires_In { get; init; }
    
    /// <summary>
    /// Идентификатор пользователя VK.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public int User_ID { get; init; }
}