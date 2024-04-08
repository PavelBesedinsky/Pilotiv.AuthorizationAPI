using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие изменения даты регистрации пользователя.
/// </summary>
/// <param name="UserId">Идентификатор пользователя.</param>
/// <param name="RegistrationDate">Дата регистрации пользователя.</param>
public record UserRegistrationDateChangedDomainEvent(UserId UserId, UserRegistrationDate RegistrationDate) : IDomainEvent;