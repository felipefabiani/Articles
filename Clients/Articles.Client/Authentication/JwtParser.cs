using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Articles.Client.Authentication;

public class JwtParser
{
    public static IEnumerable<Claim> ParseClaimsFromJWT(string jwt)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var decodedToken = jwtTokenHandler.ReadJwtToken(jwt);
        var claims = decodedToken.Claims
            .GroupBy(claim => claim.Type)
            .ToDictionary(
                 claim => claim.Key,
                 claim => string.Join(" ", claim.Select(claim => claim.Value)))
            .Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!))
            .ToList();

        return claims;
    }
}
