using FluentResults;
using Pilotiv.AuthorizationAPI.Application.Shared.Factories.Users.Dtos;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Entities;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

namespace Pilotiv.AuthorizationAPI.Application.Shared.Factories.Users;

/// <summary>
/// Фабрика пользователей VK.
/// </summary>
public class VkUserFactory : IEntityFactory<VkUser, VkUserId>
{
    private readonly UsersFactoryVkUserPayload _payload;

    /// <summary>
    /// Создание фабрики пользователей VK.
    /// </summary>
    /// <param name="payload">Объект переноса данных информации о пользователе VK для фабрики пользователей.</param>
    public VkUserFactory(UsersFactoryVkUserPayload payload)
    {
        _payload = payload;
    }

    /// <inheritdoc />
    public Result<VkUser> Create()
    {
        var getVkInternalUserIdResult = VkInternalUserId.Create(_payload.InternalUserId ?? string.Empty);
        if (getVkInternalUserIdResult.IsFailed)
        {
            return getVkInternalUserIdResult.ToResult();
        }

        var vkInternalUserId = getVkInternalUserIdResult.ValueOrDefault;

        var getVkUserResult = VkUser.Create(vkInternalUserId);
        if (getVkUserResult.IsFailed)
        {
            return getVkUserResult.ToResult();
        }

        return getVkUserResult.ValueOrDefault;
    }

    /// <inheritdoc />
    public Result<VkUser> Restore()
    {
        var vkUserId = VkUserId.Create(_payload.Id);
        
        var getVkInternalUserIdResult = VkInternalUserId.Create(_payload.InternalUserId ?? string.Empty);
        if (getVkInternalUserIdResult.IsFailed)
        {
            return getVkInternalUserIdResult.ToResult();
        }

        var vkInternalUserId = getVkInternalUserIdResult.ValueOrDefault;

        return VkUser.Restore(vkUserId, vkInternalUserId);
    }
}