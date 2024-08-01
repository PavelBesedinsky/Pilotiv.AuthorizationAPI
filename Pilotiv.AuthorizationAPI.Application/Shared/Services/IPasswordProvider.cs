namespace Pilotiv.AuthorizationAPI.Application.Shared.Services;

/// <summary>
/// Интерфейс генерации и валидации паролей.
/// </summary>
public interface IPasswordProvider
{
    /// <summary>
    /// Хэширование пароля.
    /// </summary>
    /// <param name="password">Пароль.</param>
    /// <param name="salt">Соль.</param>
    /// <returns>Хэшированный пароль.</returns>
    string HashPassword(string password, out byte[] salt);

    /// <summary>
    /// Валидация пароля.
    /// </summary>
    /// <param name="password">Валидируемый пароль.</param>
    /// <param name="hash">Захэшированный пароль.</param>
    /// <param name="salt">Соль.</param>
    /// <returns>Признак прохождения валидации.</returns>
    bool ValidatePassword(string password, string hash, byte[] salt);
}