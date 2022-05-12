using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Security.Cryptography;

public class SigningIssuerCertificate
{
    private RSA _rsa = RSA.Create();

    public RsaSecurityKey GetIssuerSigningKey()
    {
        var publicXmlKey = File.ReadAllText("./public_key.pem");
        _rsa.ImportFromPem(publicXmlKey);

        return new RsaSecurityKey(_rsa);
    }

    public SigningCredentials GetAudienceSigningKey()
    {
        var privateXmlKey = File.ReadAllText("./private_key.pem");

        _rsa.ImportFromPem(privateXmlKey);

        return new SigningCredentials(
            key: new RsaSecurityKey(_rsa),
            algorithm: SecurityAlgorithms.RsaSha256);
    }
    public void Dispose()
    {
        _rsa?.Dispose();
    }
}