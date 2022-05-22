using Articles.Helper.Extensions;
using Articles.Models.Auth;
using Articles.Models.Feature.Login;
using static BCrypt.Net.BCrypt;
using ssc = System.Security.Claims;

namespace Articles.Api.Features.Login;
public interface ILoginService
{
    Task<UserLoginResponse> Login(UserLoginRequest request, CancellationToken c);
}
public class LoginService : ILoginService, IScopedService, IAsyncDisposable, IDisposable
{
    private ArticleReadOnlyContext _context;
    private readonly ILogger<LoginService> _logger;

    public LoginService(
        IDbContextFactory<ArticleReadOnlyContext> context,
        ILogger<LoginService> logger)
    {
        _context = context?.CreateDbContext() ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync().ConfigureAwait(false);
        _context = null!;
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
                Value = DateTime.UtcNow.AddHours(4).CreateToken(
                    id: user.Id,
                    userName: $"{user.FirstName} {user.LastName}",
                    roles: e.Roles.Select(x => x.Name).ToList(),
                    claims: e.Claims
                        .Select(x => new ssc.Claim(x.Name, x.Value))
                        .ToList()
                )
            }
        };
    }
}
