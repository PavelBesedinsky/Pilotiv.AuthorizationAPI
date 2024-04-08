using Pilotiv.AuthorizationAPI.Domain.Primitives;

namespace Pilotiv.AuthorizationAPI.Application.Shared.Persistence.Commands;

/// <summary>
/// Интерфейс единицы работы.
/// </summary>
/// <typeparam name="TDomainModel">Тип доменной модели.</typeparam>
public interface IUnitOfWork<in TDomainModel> where TDomainModel : IHasDomainEvents
{
    /// <summary>
    /// Сохранение изменений доменной модели.
    /// </summary>
    /// <param name="value">Тип доменной модели.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task CommitChangesAsync(TDomainModel value, CancellationToken cancellationToken = default);
}