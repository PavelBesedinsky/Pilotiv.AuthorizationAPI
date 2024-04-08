using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Domain.Models.Users.Entities;

/// <summary>
/// Пользователь VK.
/// </summary>
public class VkUser : Entity<VkUserId>
{
    /// <summary>
    /// Создание пользователя VK.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    private VkUser(VkUserId id) : base(id)
    {
    }
}