using Pilotiv.AuthorizationAPI.Domain.Models.Users.Entities;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие изменения токена обновления, замеяющего токен.
/// </summary>
/// <param name="RefreshTokenId">Идентификатор токена доступа.</param>
/// <param name="ReplacingToken">Заменяющий токен доступа.</param>
public record RefreshTokenReplacingTokenChangedDomainEvent
    (RefreshTokenId RefreshTokenId, RefreshToken ReplacingToken) : IDomainEvent;