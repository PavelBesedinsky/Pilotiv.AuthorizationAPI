using System.Security.Cryptography;
using System.Text;
using Pilotiv.AuthorizationAPI.Application.Shared.Services;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Services.Security;

/// <summary>
/// Сервис генерации и валидации паролей.
/// </summary>
public class PasswordProvider : IPasswordProvider
{
    private readonly int _iterations;
    private readonly HashAlgorithmName _hashAlgorithm;
    private readonly int _keySize;

    public PasswordProvider()
    {
        _iterations = 100000;
        _hashAlgorithm = HashAlgorithmName.SHA512;
        _keySize = 64;
    }

    /// <inheritdoc />
    public string HashPassword(string password, out byte[] salt)
    {
        salt = RandomNumberGenerator.GetBytes(_keySize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            _iterations,
            _hashAlgorithm,
            _keySize);
        return Convert.ToHexString(hash);
    }

    /// <inheritdoc />
    public bool ValidatePassword(string password, string hash, byte[] salt)
    {
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, _hashAlgorithm, _keySize);
        return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
    }
}