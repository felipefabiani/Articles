using Articles.Client.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Articles.Client.Test;

public class FakeAuthStateProvider : AuthenticationStateProvider, IAuthStateProvider
{
    public int UserId { get => 2; }

    public override Task<AuthenticationState> GetAuthenticationStateAsync() => Task.FromResult(
        new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    public async Task NotifyUserAuthentication(UserLoginResponse userLoggedin) => await Task.CompletedTask;

    public async Task NotifyUserLogout() => await Task.CompletedTask;
}
