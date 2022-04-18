using Articles.Client.Pages;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace Articles.Client.EndPoints;
public abstract class FormBase : ComponentBase
{
    [Inject] protected IDialogService DialogService { get; set; } = null!;
    [Inject] protected ISnackbar Snackbar { get; set; } = null!;
    [Inject] protected IHttpClientFactory HttpClientFactory { get; set; } = null!;
    [Parameter] public string HttpClientName { get; set; } = null!;
    [Parameter] public string Endpoint { get; set; } = null!;
    [Parameter] public string? SuccessMessage { get; set; } = null;
    [Parameter] public string? FailedMessage { get; set; } = null;
    [Parameter] public bool DisableSuccessDefaultMessage { get; set; } = false;
    [Parameter] public bool DisableFailDefaultMessage { get; set; } = false;
    [Parameter] public string? ButtonSubmitText { get; set; } = null;

    protected CancellationTokenSource cancellationTokenSource = new();

    protected MudForm form = null!;
    protected HttpClient HttpClient { get { return HttpClientFactory.CreateClient(HttpClientName); } }
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
            Snackbar.Add($"Unexpected error:<BR/><BR>{ex.Message}", MudBlazor.Severity.Error);
        }
        finally
        {
            ResetCancelationToken();
            (await dlgRef)?.Close();
        }
    }

    protected abstract Task<HttpResponseMessage> SendMessage();

    protected virtual async Task Fail(HttpResponseMessage response)
    {
        var bad = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
        ShowFailMessage(bad);
    }
    protected virtual Task Success(HttpResponseMessage response)
    {
        ShowSuccesMessage();
        return Task.CompletedTask;
    }

    protected void ShowFailMessage(BadRequestResponse? bad)
    {
        if (FailedMessage is not null)
        {
            Snackbar.Add(
                message: FailedMessage,
                severity: Severity.Error);
        }
        else if (DisableFailDefaultMessage == false)
        {
            Snackbar.Add(
                message: FailedMessage ?? string.Join("<br/>", bad.errors.GeneralErrors),
                severity: Severity.Error);
        }
    }

    protected void ShowSuccesMessage()
    {
        if (SuccessMessage is not null)
        {
            Snackbar.Add(
                message: SuccessMessage,
                severity: Severity.Error);
        }
        else if (DisableFailDefaultMessage == false)
        {
            Snackbar.Add(
                message: "Completed successfully",
                severity: Severity.Error);
        }
    }

    private async Task<IDialogReference?> ShowDialog()
    {
        try
        {
            await Task.Delay(1_000, cancellationTokenSource.Token);
            return DialogService.Show<CancelDialog>("",
                new DialogParameters
                {
                    {"cancellationTokenSource", cancellationTokenSource }
                },
                new DialogOptions
                {
                    CloseOnEscapeKey = false,
                    DisableBackdropClick = true,
                    CloseButton = false,
                    NoHeader = true,

                });
        }
        catch (TaskCanceledException)
        {
            return null;
        }
    }

    protected virtual Task Cancel()
    {
        ResetCancelationToken();
        return Task.CompletedTask;
    }
    protected virtual Task Reset()
    {
        form.Reset();
        return Task.CompletedTask;
    }
    private void ResetCancelationToken()
    {
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
        cancellationTokenSource = new CancellationTokenSource();
    }
}
