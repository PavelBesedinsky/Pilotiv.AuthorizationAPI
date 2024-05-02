using FluentResults;
using Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users.Dtos;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.Entities;
using Pilotiv.AuthorizationAPI.Domain.Models.Users.ValueObjects;

namespace Pilotiv.AuthorizationAPI.Application.Shared.Fabrics.Users;

/// <summary>
/// Фабрика токенов доступа.
/// </summary>
public class RefreshTokenFactory : IEntityFactory<RefreshToken, RefreshTokenId>
{
    private readonly UsersFactoryRefreshTokenPayload _payload;

    /// <summary>
    /// Создание фабрики токенов доступа.
    /// </summary>
    /// <param name="payload">Объект переноса данных информации о токене обновления для фабрики пользователей.</param>
    public RefreshTokenFactory(UsersFactoryRefreshTokenPayload payload)
    {
        _payload = payload;
    }

    /// <inheritdoc />
    public Result<RefreshToken> Create()
    {
        var id = RefreshTokenId.Create(_payload.Id);

        return RefreshToken.Create(id, _payload.ExpirationDate,
            _payload.CreatedDate, _payload.CreatedByIp ?? string.Empty);
    }

    /// <inheritdoc />
    public Result<RefreshToken> Restore()
    {
        RefreshToken? replacingRefreshToken = null;
        if (_payload.ReplacingToken is not null)
        {
            var factory = new RefreshTokenFactory(_payload.ReplacingToken);
            var getReplacingTokenResult = factory.Restore();
            if (getReplacingTokenResult.IsFailed)
            {
                return getReplacingTokenResult;
            }

            replacingRefreshToken = getReplacingTokenResult.ValueOrDefault;
        }

        return RefreshToken.Restore(RefreshTokenId.Create(_payload.Id), _payload.ExpirationDate, _payload.CreatedDate,
            _payload.RevokedDate, _payload.CreatedByIp ?? string.Empty, _payload.RevokedByIp, _payload.RevokeReason,
            replacingRefreshToken);
    }
}