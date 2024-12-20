﻿using RichardSzalay.MockHttp;
using System.Net.Http;

namespace Articles.Client.Test;
public class FakeHttpClientFactory<T> : IHttpClientFactory
{

    public FakeHttpClientFactory(
        T response,
        string endpoint = "/",
        HttpStatusCode statusCode = HttpStatusCode.OK,
        string baseAddress = "http://localhost")

    {
        _baseAddress = baseAddress;
        _endpoint = endpoint;
        _response = response;
        _statusCode = statusCode;
    }
    private readonly T _response;
    private readonly HttpStatusCode _statusCode;
    private readonly string _baseAddress;
    private readonly string _endpoint;

    public HttpClient CreateClient(string name)
    {
        var respJson = JsonSerializer.Serialize(_response);
        var mockHttp = new MockHttpMessageHandler();
        var uri = $"{_baseAddress.Trim('/')}/{_endpoint.Trim('/')}";
        mockHttp
            .When(uri)
            .Respond(_statusCode, "application/json", respJson);

        var http = mockHttp.ToHttpClient();

        http.BaseAddress = new Uri(_baseAddress);
        return http;
    }
}
