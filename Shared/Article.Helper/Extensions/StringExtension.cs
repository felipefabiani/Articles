using Articles.Helper.Auth;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using ssc = System.Security.Claims;

using static BCrypt.Net.BCrypt;

namespace Articles.Helper.Extensions
{
    public static class StringExtension
    {
        public static string GetPassword(this string pwd) => EnhancedHashPassword(pwd, 12);

        public static string CreateToken(
            this DateTime expireAt,
            int id,
            string userName,
            IEnumerable<string>? roles = null,
            IEnumerable<ssc.Claim>? claims = null,
            IEnumerable<string>? permissions = null)
        {
            var list = new List<ssc.Claim>
            {
                new ssc.Claim(ssc.ClaimTypes.Name, userName),
                new ssc.Claim("id", $"{id}")
            };

            if (claims != null)
            {
                list.AddRange(claims);
            }

            if (permissions != null)
            {
                list.AddRange(permissions.Select((string p) => new ssc.Claim("permissions", p)));
            }

            if (roles != null)
            {
                list.AddRange(roles.Select((string r) => new ssc.Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", r)));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ssc.ClaimsIdentity(list),
                Expires = expireAt,
                // SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signingKey.ToString())), "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256")
                SigningCredentials = new SigningIssuerCertificate().GetAudienceSigningKey(),
                Issuer = ArticlesConstants.Security.Issuer,
                Audience = ArticlesConstants.Security.Audience
            };
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.OutboundClaimTypeMap.Clear();
            return jwtSecurityTokenHandler.WriteToken(jwtSecurityTokenHandler.CreateToken(tokenDescriptor));
        }
    }
}