using Articles.Models.Feature.Login;
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
        try
        {
            var token = await _localStorage.GetItemAsync<string>(AuthConst.AuthToken);
            if (string.IsNullOrWhiteSpace(token))
            {
                return _anonymous!;
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthConst.Bearer, token);
            return GetAuthenticationState(token);
        }
        catch (Exception ex)
        {
            return _anonymous!;
        }
    }
    public async Task NotifyUserAuthentication(UserLoginResponse userLoggedin)
    {
        try
        {
            await _localStorage.SetItemAsync(AuthConst.AuthToken, userLoggedin.Token.Value);
            var authenticatedUser = GetAuthenticationState(userLoggedin.Token.Value);
            var authState = Task.FromResult(authenticatedUser);
            NotifyAuthenticationStateChanged(authState);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthConst.Bearer, userLoggedin.Token.Value);
        }
        catch (Exception)
        {
            await NotifyUserLogout();
        }
    }

    private static AuthenticationState GetAuthenticationState(string token) => new(
        new ClaimsPrincipal(
            new ClaimsIdentity(
                JwtParser.ParseClaimsFromJWT(token),
                AuthConst.JwtAuthType)));
    public async Task NotifyUserLogout()
    {
        await _localStorage.RemoveItemAsync(AuthConst.AuthToken);

        var authState = Task.FromResult(_anonymous);
        NotifyAuthenticationStateChanged(authState!);
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
