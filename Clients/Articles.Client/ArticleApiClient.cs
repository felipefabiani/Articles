using Articles.Client.Properties.EndPoints;
using System.Net.Http.Json;

namespace Articles.Client;
public class ArticleApiClient
{
    private readonly HttpClient _http;

    public ArticleApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(
        IPostEndPoint<TRequest, TResponse> endPoint,
        CancellationToken cancellationToken = default)
        where TRequest : notnull, new()
        where TResponse : notnull, new()
    {
        var response = await _http.PostAsJsonAsync(endPoint.GetEndPoint(), endPoint.Model, cancellationToken);
        var content = await response.Content.ReadFromJsonAsync<TResponse>();

        return content ?? endPoint.NullModel;
    }
}