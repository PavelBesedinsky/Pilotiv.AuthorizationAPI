using System;

namespace Pilotiv.AuthorizationAPI.Jwt.Entities;

/// <summary>
/// Токен обновления.
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// Токен.
    /// </summary>
    public string Token { get;}
    
    /// <summary>
    /// Дата истечения.
    /// </summary>
    public DateTime Expires { get;}
    
    /// <summary>
    /// Дать создания.
    /// </summary>
    public DateTime Created { get;}
    
    /// <summary>
    /// IP пользователя, для которого создан токен.
    /// </summary>
    public string CreatedByIp { get;}

    /// <summary>
    /// Создание токена обновления.
    /// </summary>
    /// <param name="token">Токен.</param>
    /// <param name="expires">Дата истечения.</param>
    /// <param name="created">Дата создания.</param>
    /// <param name="createdByIp">IP, с которого создан токен.</param>
    public RefreshToken(string token, DateTime expires, DateTime created, string createdByIp)
    {
        Token = token;
        Expires = expires;
        Created = created;
        CreatedByIp = createdByIp;
    }
}