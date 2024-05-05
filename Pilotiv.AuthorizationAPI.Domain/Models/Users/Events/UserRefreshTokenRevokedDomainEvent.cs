using Pilotiv.AuthorizationAPI.Domain.Models.Users.Entities;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие отзыва токена обновления.
/// </summary>
/// <param name="UserId">Идентификатор пользователя, токен обновления которого был отозван.</param>
/// <param name="RevokedRefreshToken">Отозванный токен обновления.</param>
/// <param name="ReplacedRefreshToken">Заменяющий токен обновления.</param>
public record UserRefreshTokenRevokedDomainEvent(UserId UserId, RefreshToken RevokedRefreshToken,
    RefreshToken? ReplacedRefreshToken) : IDomainEvent;