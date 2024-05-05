using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие изменения IP-адреса пользователя, запрашивающего отзыв токена.
/// </summary>
/// <param name="RefreshTokenId">Идентификатор токена обновления.</param>
/// <param name="Ip">IP-адреса пользователя, запрашивающего отзыв токена.</param>
public record RefreshTokenRevokedByIpChangedDomainEvent(RefreshTokenId RefreshTokenId, string Ip) : IDomainEvent;