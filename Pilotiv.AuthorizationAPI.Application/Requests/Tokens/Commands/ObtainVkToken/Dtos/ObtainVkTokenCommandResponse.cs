namespace Pilotiv.AuthorizationAPI.Application.Requests.Tokens.Commands.ObtainVkToken.Dtos;

/// <summary>
/// Объект переноса данных результата команды получения токена VK.
/// </summary>
public class ObtainVkTokenCommandResponse
{
    /// <summary>
    /// Токен доступа.
    /// </summary>
    public string? AccessToken { get; init; }

    /// <summary>
    /// Признак, что пользователь добавлен в систему (после первого входа).
    /// </summary>
    public bool IsNew { get; init; }
}