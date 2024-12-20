using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Articles.Helper.Extensions;

namespace Articles.Client.EndPoints;
public abstract class SelectRemoteBase<TRequest, TResponse> : ComponentBase
    where TRequest : class, new()
    where TResponse : notnull, new()
{
    [Inject] protected IHttpClientFactory HttpClientFactory { get; set; } = default!;
    [Parameter] public string HttpClientName { get; set; } = default!;
    [Parameter] public string Endpoint { get; set; } = default!;
    [Parameter] public TRequest DefaultModel { get; set; } = new();
    [Parameter] public Func<TResponse, string> ToStringFunc { get; set; } = default!;
    [Parameter] public string Label { get; set; } = default!;
    [Parameter] public EventCallback<TResponse> OnValueChanged { get; set; }
    [Parameter] public EventCallback<IEnumerable<TResponse>> OnSelectedValuesChanged { get; set; }

    private TResponse _value = default!;
    public TResponse Value
    {
        get { return _value; }
        set
        {
            _value = value;

            Task.Run(async () => await OnValueChanged
                .InvokeAsync(value)
                .ConfigureAwait(false)
            );
        }
    }

    private IEnumerable<TResponse> _options = default!;
    public IEnumerable<TResponse> Options
    {
        get { return _options; }
        set { 
            _options = value;

            Task.Run(async () => await OnSelectedValuesChanged
                .InvokeAsync(value)
                .ConfigureAwait(false)
            );
        }
    }



    protected CancellationTokenSource cancellationTokenSource = new();
    protected HttpClient HttpClient { get { return HttpClientFactory.CreateClient(HttpClientName); } }
    protected TRequest _model = new();
    public List<TResponse> Response { get; set; } = new List<TResponse>();

    protected override async Task OnInitializedAsync()
    {
        await Submit();
    }
    protected async Task Submit()
    {
        try
        {
            var response = await SendMessage();

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    await Success(response);
                    break;
                case System.Net.HttpStatusCode.BadRequest:
                default:
                    await Fail(response);
                    break;
            }
        }
        finally
        {
        }
    }

    protected async Task OnChange(TResponse p)
    {
        await OnValueChanged
            .InvokeAsync(p)
            .ConfigureAwait(false);
    }

    protected async Task SelectedValuesChangedTest(HashSet<TResponse> p)
    {
        await OnSelectedValuesChanged
            .InvokeAsync()
            .ConfigureAwait(false);
    }

    protected async Task<HttpResponseMessage> SendMessage()
    {
        var endpoint = $"{Endpoint}?{_model.ToQueryString()}";

        return await HttpClient.GetAsync(endpoint, cancellationTokenSource.Token);
    }

    protected virtual async Task Fail(HttpResponseMessage response)
    {
        var bad = await response.Content.ReadFromJsonAsync<BadRequestResponse>();

    }
    protected virtual async Task Success(HttpResponseMessage response)
    {
        Response = await response.Content.ReadFromJsonAsync<List<TResponse>>() ?? new List<TResponse>();
    }
}
