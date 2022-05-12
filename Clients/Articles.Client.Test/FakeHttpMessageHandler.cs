using System.Net.Http;

namespace Articles.Client.Test;

public class FakeHttpMessageHandler : HttpMessageHandler
{
    readonly HttpResponseMessage response;

    public FakeHttpMessageHandler(HttpResponseMessage response)
    {
        this.response = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<HttpResponseMessage>();


        tcs.SetResult(response);

        return tcs.Task;
    }
}