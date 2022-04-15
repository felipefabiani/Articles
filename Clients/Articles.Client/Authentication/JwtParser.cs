using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Articles.Client.Authentication;

public class JwtParser
{
    public static IEnumerable<Claim> ParseClaimsFromJWT(string jwt)
    {
        var claims = GetClaims();

        EnsureTokenIsNotExpired();

        return claims;

        List<Claim> GetClaims()
        {
            return new JwtSecurityTokenHandler()
                .ReadJwtToken(jwt)
                .Claims
                .GroupBy(claim => claim.Type)
                .ToDictionary(
                     claim => claim.Key,
                     claim => string.Join(" ", claim.Select(claim => claim.Value)))
                .Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!))
                .ToList();
        }

        void EnsureTokenIsNotExpired()
        {
            var expClaim = long.Parse(claims.First(c => c.Type == "exp").Value);
            var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            if (expClaim < timestamp)
            {
                throw new Exception("Token Expired!");
            }
        }
    }
}
