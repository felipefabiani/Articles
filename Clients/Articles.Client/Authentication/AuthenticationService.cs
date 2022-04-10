using Articles.Models.Feature.Login;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

namespace Articles.Client.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _client;
    private readonly AuthStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthenticationService(
        HttpClient client,
        AuthenticationStateProvider authStateProvider,
        ILocalStorageService localStorage
        )
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _authStateProvider = (AuthStateProvider)authStateProvider ?? throw new ArgumentNullException(nameof(authStateProvider));
        _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
    }

    public async Task<UserLoginResponse> Login(UserLoginResponse userLoggedin)
    {

        await _localStorage.SetItemAsync(AuthConst.AuthToken, userLoggedin.Token.Value);
        _authStateProvider.NotifyUserAuthentication(userLoggedin.Token.Value);

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthConst.Bearer, userLoggedin.Token.Value);

        return userLoggedin;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync(AuthConst.AuthToken);
        _authStateProvider.NotifyUserLogout();
        _client.DefaultRequestHeaders.Authorization = null;
    }
}
