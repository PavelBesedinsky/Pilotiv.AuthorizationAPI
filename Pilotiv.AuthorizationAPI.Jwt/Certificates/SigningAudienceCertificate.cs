using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Pilotiv.AuthorizationAPI.Jwt.Certificates;

/// <summary>
/// Вспомогательный класс создания секретного ключа.
/// </summary>
public class SigningAudienceCertificate : IDisposable
{
    private readonly RSA _rsa;

    /// <summary>
    /// Вспомогательный класс создания секретного ключа.
    /// </summary>
    public SigningAudienceCertificate()
    {
        _rsa = RSA.Create();
    }

    /// <summary>
    /// Получение секретного ключа.
    /// </summary>
    /// <param name="privateKey">RSA Private Key.</param>
    /// <returns>Подписанный публичный ключ.</returns>
    public SigningCredentials GetAudienceSigningKey(string privateKey)
    {
        _rsa.ImportFromPem(privateKey);

        return new SigningCredentials(
            key: new RsaSecurityKey(_rsa),
            algorithm: SecurityAlgorithms.RsaSha256);
    }

    /// <summary>
    /// Освобождение ресурсов.
    /// </summary>
    public void Dispose()
    {
        _rsa.Dispose();
    }
}