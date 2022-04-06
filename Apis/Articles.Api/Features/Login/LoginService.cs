using BCrypt.Net;
using ssc = System.Security.Claims;

using static BCrypt.Net.BCrypt;
using Articles.Models.Feature.Login;
using Articles.Models.Auth;

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
            .FirstOrDefaultAsync(cancellationToken) ?? new User();

        if (
            string.IsNullOrWhiteSpace(user.Password) ||
            !EnhancedVerify(request.Password, user.Password, HashType.SHA512))
        {
            return NullUserLoginResponse.Empty;
        }

        return MapFromEntityAsync(user);

        UserLoginResponse MapFromEntityAsync(User e) => new()
        {
            FullName = $"{e.FirstName} {e.LastName}",
            UserRoles = e.Roles.Select(x => x.Name).ToList(),
            UserClaims = e.Claims.Select(x => x.Name).ToList(),
            Token = new JwtToken
            {
                ExpiryDate = DateTime.UtcNow.AddHours(4),
                Value = JWTBearer.CreateToken(
                    signingKey: _options.JwtSigningKey,
                    expireAt: DateTime.UtcNow.AddHours(4),
                    roles: e.Roles.Select(x => x.Name).ToList(),
                    claims: e.Claims
                        .Select(x => new ssc.Claim(x.Name, x.Value))
                        .ToList()
                )
            }
        };
    }
}
