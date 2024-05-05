namespace Pilotiv.AuthorizationAPI.Domain.Primitives;

/// <summary>
/// Примитив объекта-значения
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// Получение признака равенства объектов.
    /// </summary>
    /// <param name="other">Сравниваемый объект-значение.</param>
    /// <returns>Признак равенства объектов-значений.</returns>
    public bool Equals(ValueObject? other)
    {
        return Equals((object?)other);
    }
    
    /// <summary>
    /// Получение сравниваемых компонентов объекта.
    /// </summary>
    /// <returns>Сравниваемые компоненты объекта.</returns>
    protected abstract IEnumerable<object> GetEqualityComponents();

    /// <summary>
    /// Получение признака равенства объектов.
    /// </summary>
    /// <param name="obj">Сравниваемый объект.</param>
    /// <returns>Признак равенства объектов</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not ValueObject valueObject)
        {
            return false;
        }
        
        return GetEqualityComponents()
            .SequenceEqual(valueObject.GetEqualityComponents());
    }

    /// <summary>
    /// Перегрузка оператора "==".
    /// </summary>
    /// <param name="left">Значение слева от оператора.</param>
    /// <param name="right">Значение справа от оператора.</param>
    /// <returns>Признак раветства объектов.</returns>
    public static bool operator ==(ValueObject left, ValueObject right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Перегрузка оператора "!=".
    /// </summary>
    /// <param name="left">Значение слева от оператора.</param>
    /// <param name="right">Значение справа от оператора.</param>
    /// <returns>Признак нераветства объектов.</returns>
    public static bool operator !=(ValueObject left, ValueObject right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    /// Получение хэш-кода.
    /// </summary>
    /// <returns>Хэш-код.</returns>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x.GetHashCode())
            .Aggregate((x, y) => x ^ y);
    }
}