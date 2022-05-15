using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ssc = System.Security.Claims;

namespace Articles.Test.Helper.Extensions;
public static class HelperExtension
{
    public static string CreateToken(
        this Guid signingKey,
        string firstName,
        string lastName,
        DateTime? expireAt = null,
        IEnumerable<string>? permissions = null,
        IEnumerable<string>? roles = null,
        IEnumerable<ssc.Claim>? claims = null)
    {
        var list = new List<ssc.Claim>();
        if (claims != null)
        {
            list.AddRange(claims);
            list.Add(new ssc.Claim(ssc.ClaimTypes.Name, $"{firstName} {lastName}"));
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
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signingKey.ToString())), "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256")
        };
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        jwtSecurityTokenHandler.OutboundClaimTypeMap.Clear();
        return jwtSecurityTokenHandler.WriteToken(jwtSecurityTokenHandler.CreateToken(tokenDescriptor));
    }

}
