using Pilotiv.AuthorizationAPI.Domain.Models.Users.Entities;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие добавления токена обновления к пользователю.
/// </summary>
/// <param name="UserId">Идентификатор пользователя.</param>
/// <param name="RefreshToken">Токен обновления.</param>
public record UserRefreshTokenAddedDomainEvent(UserId UserId, RefreshToken RefreshToken) : IDomainEvent;