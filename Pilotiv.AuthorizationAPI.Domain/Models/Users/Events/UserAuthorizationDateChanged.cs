using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие изменения даты авторизации пользователя.
/// </summary>
/// <param name="UserId">Идентификатор пользователя.</param>
/// <param name="AuthorizationDate">Дата авторизации пользователя.</param>
public record UserAuthorizationDateChanged(UserId UserId, UserAuthorizationDate AuthorizationDate) : IDomainEvent;