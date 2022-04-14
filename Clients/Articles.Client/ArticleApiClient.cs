using Articles.Client.EndPoints;
using Articles.Models.Feature.Login;
using CSharpFunctionalExtensions;
using System.Net.Http.Json;

namespace Articles.Client;
public class ArticleApiClient
{
    private readonly HttpClient _http;

    public ArticleApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<(TResponse, BadRequestResponse)> PostAsync<TRequest, TResponse>(
        IPostEndPoint<TRequest, TResponse> endPoint,
        CancellationToken cancellationToken = default)
        where TRequest : notnull, new()
        where TResponse : notnull, new()
    {
        try
        {
            var response = await _http.PostAsJsonAsync(endPoint.GetEndPoint(), endPoint.Model, cancellationToken);

            // response.EnsureSuccessStatusCode();

            BadRequestResponse badRequest;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                badRequest = await response.Content.ReadFromJsonAsync<BadRequestResponse>();

            }

            var content = await response.Content.ReadFromJsonAsync<TResponse>();

            return (content ?? endPoint.Response, null);
        }
        catch (System.Net.Http.HttpRequestException ex)
        {
            throw;
        }
    }



    public class BadRequestResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public Errors errors { get; set; }
    }

    public class Errors
    {
        public string[] GeneralErrors { get; set; }
    }

}