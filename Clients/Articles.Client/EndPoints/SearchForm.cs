using Articles.Client.Shared.Templates;
using Articles.Helper.Extensions;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Articles.Client.EndPoints;
public abstract class SearchFormBase<TRequest, TResponse> : FormBase<TRequest, TResponse>
    where TRequest : class, new()
    where TResponse : notnull, new()
{
    [Parameter] public Table<TResponse> Table { get; set; } = default!;

    protected string searchString1 = string.Empty;
    // public MudTable<TResponse> mudTable = new();
    protected override async Task<HttpResponseMessage> SendMessage()
    {
        var endpoint = $"{Endpoint}?{_model.ToQueryString()}";

        return await HttpClient.GetAsync(endpoint, cancellationTokenSource.Token);
    }

    public TResponse _resp = default!;
    public List<TResponse>? Response { get; set; }
    protected override async Task Success(HttpResponseMessage response)
    {
        Response = await response.Content.ReadFromJsonAsync<List<TResponse>>();

        ShowSuccesMessage();

        SuccessCallBack?.Invoke(_resp);
    }

    protected override Task Reset()
    {
        searchString1 = string.Empty;
        Response = default!;
        return base.Reset();
    }
}
