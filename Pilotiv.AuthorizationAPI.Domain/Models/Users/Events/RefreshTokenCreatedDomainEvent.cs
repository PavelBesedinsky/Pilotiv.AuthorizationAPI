using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие создания токена обновления.
/// </summary>
/// <param name="RefreshTokenId">Идентификатор токена обновления.</param>
public record RefreshTokenCreatedDomainEvent(RefreshTokenId RefreshTokenId) : IDomainEvent;