using Articles.Client.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace Articles.Client.Test;

public class FakeAuthStateProvider : AuthenticationStateProvider, IAuthStateProvider
{
    public int UserId { get => 2; }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        throw new NotImplementedException();
    }

    public Task NotifyUserAuthentication(UserLoginResponse userLoggedin)
    {
        throw new NotImplementedException();
    }

    public Task NotifyUserLogout()
    {
        throw new NotImplementedException();
    }
}
