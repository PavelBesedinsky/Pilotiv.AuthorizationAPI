using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие изменения даты истечения токена.
/// </summary>
/// <param name="RefreshTokenId">Идентификатор токена обновления.</param>
/// <param name="ExpirationDate">Дата истечения токена.</param>
public record RefreshTokenExpirationDateChangedDomainEvent
    (RefreshTokenId RefreshTokenId, DateTime ExpirationDate) : IDomainEvent;