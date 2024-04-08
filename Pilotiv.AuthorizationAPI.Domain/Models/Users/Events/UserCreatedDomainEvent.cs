using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие создания пользователя.
/// </summary>
/// <param name="User">Созданный пользователь.</param>
public record UserCreatedDomainEvent(User User) : IDomainEvent;
