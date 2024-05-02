using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие изменения даты отзыва токена.
/// </summary>
/// <param name="RefreshTokenId">Идентификатор токена обновления.</param>
/// <param name="RevokedDate">Дата истечения токена.</param>
public record RefreshTokenRevokedDateChangedDomainEvent
    (RefreshTokenId RefreshTokenId, DateTime RevokedDate) : IDomainEvent;