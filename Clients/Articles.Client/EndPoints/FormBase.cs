using Articles.Client.Pages;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using static Articles.Client.ArticleApiClient;

namespace Articles.Client.EndPoints;
public abstract class FormBase<TRequest, TResponse> : ComponentBase
    where TRequest : class, new()
    where TResponse : notnull, new()
{
    [Inject] protected IDialogService DialogService { get; set; } = null!;
    [Inject] protected ISnackbar Snackbar { get; set; } = null!;
    [Inject] protected IServiceProvider ServiceProvider { get; set; } = null!;
    [Inject] protected AbstractValidator<TRequest> Validator { get; set; } = null!;

    [Parameter] public string HttpClientName { get; set; } = null!;
    [Parameter] public IPostEndPoint<TRequest, TResponse> Endpoint { get; set; } = null!;
    [Parameter] public TRequest Model { get; set; } = new TRequest();
    [Parameter] public RenderFragment HeaderTemplate { get; set; } = default!;
    [Parameter] public RenderFragment<TRequest> FormTemplate { get; set; } = default!;
    [Parameter] public RenderFragment ButtonsTemplate { get; set; } = default!;
    [Parameter] public string? SuccessMessage { get; set; }
    [Parameter] public string? FailedMessage { get; set; }
    [Parameter] public bool DisableSuccessDefaultMessage { get; set; } = false;
    [Parameter] public bool DisableFailDefaultMessage { get; set; } = false;
    [Parameter] public Action<TResponse>? SuccessCallBack { get; set; }
    [Parameter] public Action<BadRequestResponse>? FailCallBack { get; set; }

    protected CancellationTokenSource cancellationTokenSource = new();

    protected MudForm form = null!;
    private HttpClient HttpClient { get { return ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(HttpClientName); } }
    protected async Task Submit()
    {
        await form.Validate();


        if (!form.IsValid)
        {
            return;
        }

        var dlgRef = ShowDialog();

        try
        {
            var response = await HttpClient.PostAsJsonAsync(Endpoint.GetEndPoint(), Endpoint.Model, cancellationTokenSource.Token);

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
        catch (TaskCanceledException ex) when (ex.CancellationToken.IsCancellationRequested)
        {
            Snackbar.Add("User canceled request!", MudBlazor.Severity.Warning);
        }
        catch (TaskCanceledException)
        {
            Snackbar.Add("Request timed out", MudBlazor.Severity.Warning);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Unexpected error<BR/><BR>{ex.Message}", MudBlazor.Severity.Error);
        }
        finally
        {
            dlgRef.Close();
            ResetCancelationToken();
        }
    }

    private async Task Fail(HttpResponseMessage response)
    {
        var bad = await response.Content.ReadFromJsonAsync<BadRequestResponse>();

        if (FailedMessage is not null)
        {
            Snackbar.Add(
                message: FailedMessage,
                severity: MudBlazor.Severity.Error);
        }
        else if(DisableFailDefaultMessage == false)
        {
            Snackbar.Add(
                message: FailedMessage ?? string.Join("<br/>", bad.errors.GeneralErrors),
                severity: MudBlazor.Severity.Error);
        }
        FailCallBack?.Invoke(bad);
    }

    private async Task Success(HttpResponseMessage response)
    {
        var result = await response.Content.ReadFromJsonAsync<TResponse>();

        if (FailedMessage is not null)
        {
            Snackbar.Add(
                message: SuccessMessage,
                severity: MudBlazor.Severity.Success);
        }
        else if (DisableFailDefaultMessage == false)
        {
            Snackbar.Add(
                message: "Completed successfully",
                severity: MudBlazor.Severity.Success);
        }
        SuccessCallBack?.Invoke(result);
    }

    private IDialogReference ShowDialog()
    {
        return DialogService.Show<CancelDialog>("",
            new DialogParameters
            {
                    {"cancellationTokenSource", cancellationTokenSource }
            },
            new DialogOptions
            {
                CloseOnEscapeKey = false,
                DisableBackdropClick = true,
                CloseButton = false
            });
    }

    protected async Task Cancel()
    {
        cancellationTokenSource.Cancel();
        ResetCancelationToken();
        await Task.CompletedTask;
    }
    protected Task Reset()
    {
        form.Reset();
        Endpoint.Model = Model.CloneJson(); ;
        return Task.CompletedTask;
    }
    protected override Task OnInitializedAsync()
    {
        Endpoint.Model = Model.CloneJson();
        return base.OnInitializedAsync();
    }

    public Func<object, string, IEnumerable<string>> ValidateValue => (mod, propertyName) =>
    {
        var result = Validator.Validate(ValidationContext<TRequest>.CreateWithOptions((TRequest)mod, x => x.IncludeProperties(propertyName)));
        return
            result.IsValid ?
            (IEnumerable<string>)Array.Empty<string>() :
            result.Errors.Select(e => e.ErrorMessage);
    };

    private void ResetCancelationToken()
    {
        cancellationTokenSource.Dispose();
        cancellationTokenSource = new CancellationTokenSource();
    }
}
