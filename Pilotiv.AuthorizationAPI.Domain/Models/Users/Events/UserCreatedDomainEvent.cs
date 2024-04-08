using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие создания пользователя.
/// </summary>
/// <param name="UserId">Идентификатор созданного пользователя.</param>
public record UserCreatedDomainEvent(UserId UserId) : IDomainEvent;
