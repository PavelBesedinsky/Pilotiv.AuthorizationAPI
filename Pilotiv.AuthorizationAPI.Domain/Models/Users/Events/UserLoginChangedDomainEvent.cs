using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие изменения логина пользователя.
/// </summary>
/// <param name="UserId">Идентификатор пользователя.</param>
/// <param name="UserLogin">Логин пользователя.</param>
public record UserLoginChangedDomainEvent(UserId UserId, UserLogin UserLogin) : IDomainEvent;