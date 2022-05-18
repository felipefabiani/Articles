using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace Articles.Client.Authentication;
public class AuthenticationHttpMessageHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public AuthenticationHttpMessageHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
    }
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await _localStorage
            .GetItemAsync<string>(AuthConst.AuthToken)
            .ConfigureAwait(false);

        request.Headers.Authorization =
            new AuthenticationHeaderValue(AuthConst.Bearer, token);

        return await base
            .SendAsync(request, cancellationToken)
            .ConfigureAwait(false);
    }
}
