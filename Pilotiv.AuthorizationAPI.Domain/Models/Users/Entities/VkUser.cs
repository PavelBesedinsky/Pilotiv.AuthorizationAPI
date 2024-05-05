using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Events;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Entities;

/// <summary>
/// Пользователь VK.
/// </summary>
public class VkUser : Entity<VkUserId>
{
    /// <summary>
    /// Внутренний идентификатор пользователя в VK.
    /// </summary>
    public VkInternalUserId InternalId { get; }

    /// <summary>
    /// Создание пользователя VK.
    /// </summary>
    /// <param name="internalUserId">Внутренний идентификатор пользователя в VK.</param>
    private VkUser(VkInternalUserId internalUserId) : base(VkUserId.Create())
    {
        InternalId = internalUserId;
    }

    /// <summary>
    /// Создание пользователя VK.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="internalUserId">Внутренний идентификатор пользователя в VK.</param>
    private VkUser(VkUserId id, VkInternalUserId internalUserId) : base(id)
    {
        InternalId = internalUserId;
    }

    /// <summary>
    /// Создание пользователя VK.
    /// </summary>
    /// <param name="internalUserId">Внутренний идентификатор пользователя в VK.</param>
    /// <returns>Пользователь VK.</returns>
    public static Result<VkUser> Create(VkInternalUserId internalUserId)
    {
        var vkUser = new VkUser(internalUserId);

        vkUser.AddDomainEvent(new VkUserCreatedDomainEvent(vkUser.Id));
        vkUser.AddDomainEvent(new VkUserInternalIdChangedDomainEvent(vkUser.Id, internalUserId));

        return vkUser;
    }

    /// <summary>
    /// Восстановление пользователя VK.
    /// </summary>
    /// <param name="id">Идентификатор пользователя VK.</param>
    /// <param name="internalUserId">Внутренний идентификатор пользователя в VK.</param>
    /// <returns>Пользователь VK.</returns>
    public static Result<VkUser> Restore(VkUserId id, VkInternalUserId internalUserId)
    {
        return new VkUser(id, internalUserId);
    }
}