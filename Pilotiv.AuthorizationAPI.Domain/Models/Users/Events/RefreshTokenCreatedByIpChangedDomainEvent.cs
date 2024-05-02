using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие изменения IP-адреса пользователя, запрашивающего создание токена.
/// </summary>
/// <param name="RefreshTokenId">Идентификатор токена обновления.</param>
/// <param name="Ip">IP-адреса пользователя, запрашивающего создание токена.</param>
public record RefreshTokenCreatedByIpChangedDomainEvent(RefreshTokenId RefreshTokenId, string Ip) : IDomainEvent;