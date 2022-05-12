using Articles.Models.Auth;
using Articles.Models.Feature.Login;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using static BCrypt.Net.BCrypt;
using ssc = System.Security.Claims;

namespace Articles.Api.Features.Login;
public interface ILoginService
{
    Task<UserLoginResponse> Login(UserLoginRequest request, CancellationToken c);
}
public class LoginService : ILoginService, IScopedService
{
    private readonly ArticleReadOnlyContext _context;
    private readonly ArticleOptions _options;
    private readonly ILogger<LoginService> _logger;

    public LoginService(
        ArticleReadOnlyContext context,
        IOptions<ArticleOptions> options,
        ILogger<LoginService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<UserLoginResponse> Login(UserLoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Where(x => x.Email == request.Email)
            .Include(x => x.Roles)
            .Include(x => x.Claims)
            .FirstOrDefaultAsync(cancellationToken) ?? new UserEntity();

        if (
            string.IsNullOrWhiteSpace(user.Password) ||
            !EnhancedVerify(request.Password, user.Password))
        {
            return NullUserLoginResponse.Empty;
        }

        return MapFromEntityAsync(user);

        UserLoginResponse MapFromEntityAsync(UserEntity e) => new()
        {
            FullName = $"{e.FirstName} {e.LastName}",
            UserRoles = e.Roles.Select(x => x.Name).ToList(),
            UserClaims = e.Claims.Select(x => x.Name).ToList(),
            Token = new JwtToken
            {
                ExpiryDate = DateTime.UtcNow.AddHours(4),
                Value = CreateToken(
                    signingKey: _options.JwtSigningKey,
                    expireAt: DateTime.UtcNow.AddHours(4),
                    roles: e.Roles.Select(x => x.Name).ToList(),
                    // permissions: e.Claims.Select(x => x.Value).ToList(),
                    claims: e.Claims
                        .Select(x => new ssc.Claim(x.Name, x.Value))
                        .ToList()
                )
            }
        };
        string CreateToken(string signingKey, DateTime? expireAt = null, IEnumerable<string>? permissions = null, IEnumerable<string>? roles = null, IEnumerable<ssc.Claim>? claims = null)
        {
            var list = new List<ssc.Claim>
            {
                new ssc.Claim(ssc.ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new ssc.Claim("id", $"{user.Id}")
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
                // SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signingKey)), "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256")
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
