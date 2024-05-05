using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Pilotiv.AuthorizationAPI.Jwt.Certificates;

/// <summary>
/// Вспомогательный класс создания публичного ключа.
/// </summary>
public class SigningIssuerCertificate : IDisposable
{
    private readonly RSA _rsa;

    /// <summary>
    /// Вспомогательный класс создания секретного ключа.
    /// </summary>
    public SigningIssuerCertificate()
    {
        _rsa = RSA.Create();
    }

    /// <summary>
    /// Получение публичного ключа.
    /// </summary>
    /// <param name="publicKey">RSA Public Key.</param>
    /// <returns>Подписанный секретный ключ.</returns>
    public RsaSecurityKey GetIssuerSingingKey(string publicKey)
    {
        _rsa.ImportFromPem(publicKey);

        return new RsaSecurityKey(_rsa);
    }

    /// <summary>
    /// Освобождение ресурсов.
    /// </summary>
    public void Dispose()
    {
        _rsa.Dispose();
    }
}