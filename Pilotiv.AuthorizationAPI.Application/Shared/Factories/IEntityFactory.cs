using FluentResults;
using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Application.Shared.Factories;

/// <summary>
/// Интерфейс фабрики.
/// </summary>
public interface IEntityFactory<TEntity, in TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : ValueObject
{
    /// <summary>
    /// Создание нового агрегата.
    /// </summary>
    public Result<TEntity> Create();

    /// <summary>
    /// Востановление агрегата.
    /// </summary>
    public Result<TEntity> Restore();
}