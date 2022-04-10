using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Articles.Client.Authentication;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationState _anonymous = new(
        new ClaimsPrincipal(new ClaimsIdentity()));

    public AuthStateProvider(
        HttpClient httpClient,
        ILocalStorageService localStorage)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
            var token = await _localStorage.GetItemAsync<string>(AuthConst.AuthToken);
        try
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return _anonymous!;
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthConst.Bearer, token);

        }
        catch (Exception ex)
        {

            throw;
        }
        return GetAuthenticationState(token);
    }
    public void NotifyUserAuthentication(string token)
    {
        var authenticatedUser = GetAuthenticationState(token);
        var authState = Task.FromResult(authenticatedUser);
        NotifyAuthenticationStateChanged(authState);
    }

    private static AuthenticationState GetAuthenticationState(string token) => new (
        new ClaimsPrincipal(
            new ClaimsIdentity(
                JwtParser.ParseClaimsFromJWT(token),
                AuthConst.JwtAuthType)));
    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(_anonymous);
        NotifyAuthenticationStateChanged(authState!);
    }
}
