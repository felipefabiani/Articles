using Articles.Models.Feature.Login;
using Microsoft.AspNetCore.Components.Authorization;

namespace Articles.Client.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly AuthStateProvider _authStateProvider;

    public AuthenticationService(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = (AuthStateProvider)authStateProvider ?? throw new ArgumentNullException(nameof(authStateProvider));
    }

    public async Task<UserLoginResponse> Login(UserLoginResponse userLoggedin)
    {
        await _authStateProvider.NotifyUserAuthentication(userLoggedin);
        return userLoggedin;
    }

    public async Task Logout()
    {
        await _authStateProvider.NotifyUserLogout();
    }
}
