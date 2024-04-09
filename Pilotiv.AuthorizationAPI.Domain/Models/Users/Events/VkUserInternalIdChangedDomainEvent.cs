using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;

/// <summary>
/// Событие изменения внутреннего идентификатор пользователя в VK.
/// </summary>
/// <param name="VkUserId">Идентификатор пользователя VK.</param>
/// <param name="InternalUserId">Измененный внутренний идентификатор пользователя в VK.</param>
public record VkUserInternalIdChangedDomainEvent(VkUserId VkUserId, VkInternalUserId InternalUserId) : IDomainEvent;