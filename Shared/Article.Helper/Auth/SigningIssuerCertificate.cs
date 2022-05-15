using Microsoft.IdentityModel.Tokens;
using PemUtils;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace Articles.Helper.Auth
{
    public class SigningIssuerCertificate
    {
        private RSA _rsa = RSA.Create();

        public RsaSecurityKey GetIssuerSigningKey()
        {
            ImportPemFromFile("public_key.pem");

            return new RsaSecurityKey(_rsa);
        }

        public SigningCredentials GetAudienceSigningKey()
        {
            ImportPemFromFile("private_key.pem");

            return new SigningCredentials(
                key: new RsaSecurityKey(_rsa),
                algorithm: SecurityAlgorithms.RsaSha256);
        }
        private void ImportPemFromFile(string pem)
        {
            var path = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                pem);

            using var stream = File.OpenRead(path);
            using var reader = new PemReader(stream);
            var rsaParameters = reader.ReadRsaKey();
            _rsa.ImportParameters(rsaParameters);
        }
        public void Dispose()
        {
            _rsa?.Dispose();
        }
    }
}
