using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Entities;

/// <summary>
/// Токен обновления.
/// </summary>
public class RefreshToken : Entity<RefreshTokenId>
{
    /// <summary>
    /// Дата истечения токена.
    /// </summary>
    public DateTime ExpirationDate { get; private set; }

    /// <summary>
    /// Дата создания токена.
    /// </summary>
    public DateTime CreatedDate { get; private set; }

    /// <summary>
    /// Дата отзыва токена.
    /// </summary>
    public DateTime RevokedDate { get; private set; }

    /// <summary>
    /// IP-адрес пользователя, запрашивающего создание токена.
    /// </summary>
    public string CreatedByIp { get; private set; }

    /// <summary>
    /// IP-адрес пользователя, запрашивающего отзыв токена.
    /// </summary>
    public string? RevokedByIp { get; private set; }

    /// <summary>
    /// Причина отзыва токена.
    /// </summary>
    public string? RevokeReason { get; private set; }

    /// <summary>
    /// Токен обновления, заменяющий текущий токен.
    /// </summary>
    public RefreshToken? ReplacingToken { get; private set; }

    /// <summary>
    /// Признак, что время жизни токена истекло
    /// </summary>
    public bool IsExpired => DateTime.UtcNow >= ExpirationDate;

    /// <summary>
    /// Признак, что токен был отозван
    /// </summary>
    public bool IsRevoked => RevokedDate != DateTime.MinValue;

    /// <summary>
    /// Признак, что токен активен
    /// </summary>
    public bool IsActive => !IsRevoked && !IsExpired;

    /// <summary>
    /// Создание токена обновления.
    /// </summary>
    /// <param name="id">Идентификатор токена обновления.</param>
    /// <param name="expirationDate">Дата истечения токена.</param>
    /// <param name="createdDate">Дата создания токена.</param>
    /// <param name="revokedDate">Дата отзыва токена.</param>
    /// <param name="createdByIp">IP-адрес пользователя, запрашивающего создание токена.</param>
    /// <param name="revokedByIp">IP-адрес пользователя, запрашивающего отзыв токена.</param>
    /// <param name="revokeReason">Причина отзыва токена.</param>
    /// <param name="replacingToken">Токен обновления, заменяющий текущий токен.</param>
    private RefreshToken(RefreshTokenId id, DateTime expirationDate, DateTime createdDate,
        DateTime revokedDate, string createdByIp, string? revokedByIp, string? revokeReason,
        RefreshToken? replacingToken) : base(id)
    {
        ExpirationDate = expirationDate;
        CreatedDate = createdDate;
        RevokedDate = revokedDate;
        CreatedByIp = createdByIp;
        RevokedByIp = revokedByIp;
        RevokeReason = revokeReason;
        ReplacingToken = replacingToken;
    }

    /// <summary>
    /// Создание токен аобновления.
    /// </summary>
    /// <param name="id">Идентификатор токена обновления.</param>
    /// <param name="expirationDate">Дата истечения токена.</param>
    /// <param name="createdDate">Дата создания токена.</param>
    /// <param name="createdByIp">IP-адрес пользователя, запрашивающего создание токена.</param>
    private RefreshToken(RefreshTokenId id, DateTime expirationDate, DateTime createdDate,
        string createdByIp) : this(id, expirationDate, createdDate, DateTime.MinValue, createdByIp, null, null, null)
    {
    }

    /// <summary>
    /// Создание токена обновления.
    /// </summary>
    /// <param name="id">Идентификатор токена обновления.</param>
    /// <param name="expirationDate">Дата истечения токена.</param>
    /// <param name="createdDate">Дата создания токена.</param>
    /// <param name="createdByIp">IP-адрес пользователя, запрашивающего создание токена.</param>
    /// <returns>Токен обновления.</returns>
    public static Result<RefreshToken> Create(RefreshTokenId id, DateTime expirationDate,
        DateTime createdDate,
        string createdByIp)
    {
        var refreshToken = new RefreshToken(id, expirationDate, createdDate, createdByIp);

        refreshToken.AddDomainEvent(new RefreshTokenCreatedDomainEvent(id));
        refreshToken.AddDomainEvent(new RefreshTokenExpirationDateChangedDomainEvent(id, expirationDate));
        refreshToken.AddDomainEvent(new RefreshTokenCreatedDateChangedDomainEvent(id, createdDate));
        refreshToken.AddDomainEvent(new RefreshTokenCreatedByIpChangedDomainEvent(id, createdByIp));

        return refreshToken;
    }

    /// <summary>
    /// Восстановление токена обновления.
    /// </summary>
    /// <param name="id">Идентификатор токена обновления.</param>
    /// <param name="expirationDate">Дата истечения токена.</param>
    /// <param name="createdDate">Дата создания токена.</param>
    /// <param name="revokedDate">Дата отзыва токена.</param>
    /// <param name="createdByIp">IP-адрес пользователя, запрашивающего создание токена.</param>
    /// <param name="revokedByIp">IP-адрес пользователя, запрашивающего отзыв токена.</param>
    /// <param name="revokeReason">Причина отзыва токена.</param>
    /// <param name="replacingToken">Токен обновления, заменяющий текущий токен.</param>
    /// <returns>Токен обновления.</returns>
    public static Result<RefreshToken> Restore(RefreshTokenId id, DateTime expirationDate,
        DateTime createdDate, DateTime revokedDate, string createdByIp, string? revokedByIp, string? revokeReason,
        RefreshToken? replacingToken)
    {
        return new RefreshToken(id, expirationDate, createdDate, revokedDate, createdByIp, revokedByIp,
            revokeReason, replacingToken);
    }

    /// <summary>
    /// Отзыв токена.
    /// </summary>
    /// <param name="ip">IP-адрес пользователя, запрашивающего отзыв токена.</param>
    /// <param name="reason">Причина отзыва токена.</param>
    /// <param name="replacingToken">Отзывающий токен.</param>
    internal Result RevokeToken(string ip, string reason, RefreshToken? replacingToken = null)
    {
        if (IsRevoked)
        {
            return Result.Ok();
        }

        RevokedDate = DateTime.UtcNow;
        RevokedByIp = ip;
        RevokeReason = reason;
        ReplacingToken = replacingToken;

        AddDomainEvent(new RefreshTokenRevokedDateChangedDomainEvent(Id, RevokedDate));
        AddDomainEvent(new RefreshTokenRevokedByIpChangedDomainEvent(Id, RevokedByIp));
        AddDomainEvent(new RefreshTokenRevokeReasonChangedDomainEvent(Id, RevokeReason));

        if (ReplacingToken is not null)
        {
            AddDomainEvent(new RefreshTokenReplacingTokenChangedDomainEvent(Id, ReplacingToken));
        }

        return Result.Ok();
    }
}