using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие создания пользователя VK.
/// </summary>
/// <param name="Id">Идентификатор созданного пользователя VK.</param>
public record VkUserCreatedDomainEvent(VkUserId Id) : IDomainEvent;