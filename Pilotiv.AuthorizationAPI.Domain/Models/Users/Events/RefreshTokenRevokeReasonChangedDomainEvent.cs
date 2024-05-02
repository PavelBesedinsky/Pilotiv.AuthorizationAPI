using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие изменения причины отзыва токена обновления.
/// </summary>
/// <param name="RefreshTokenId">Идентификатор токена обновления.</param>
/// <param name="Reason">Причина отзыва токена.</param>
public record RefreshTokenRevokeReasonChangedDomainEvent(RefreshTokenId RefreshTokenId, string Reason) : IDomainEvent;