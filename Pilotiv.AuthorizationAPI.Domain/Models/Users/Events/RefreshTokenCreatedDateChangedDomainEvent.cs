using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие изменения даты создания токена.
/// </summary>
/// <param name="RefreshTokenId">Идентификатор токена обновления.</param>
/// <param name="CreatedDate">Дата создания токена.</param>
public record RefreshTokenCreatedDateChangedDomainEvent
    (RefreshTokenId RefreshTokenId, DateTime CreatedDate) : IDomainEvent;