using Pilotiv.AuthorizationAPI.Domain.Models.Users.Entities;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие изменения пользователя VK.
/// </summary>
/// <param name="UserId">Идентификатор пользователя.</param>
/// <param name="VkUser">Пользователь VK.</param>
public record UserVkUserChangedDomainEvent(UserId UserId, VkUser VkUser) : IDomainEvent;