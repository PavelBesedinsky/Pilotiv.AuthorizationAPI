using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие изменения адреса электронной почты пользователя.
/// </summary>
/// <param name="UserId">Идентификатор пользователя.</param>
/// <param name="UserEmail">Адрес электронной почты пользователя.</param>
public record UserEmailChangedDomainEvent(UserId UserId, UserEmail UserEmail) : IDomainEvent;