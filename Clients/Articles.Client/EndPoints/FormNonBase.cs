using Articles.Client.Pages;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using Toolbelt.Blazor.HotKeys;

namespace Articles.Client.EndPoints;
public abstract class FormBase : ComponentBase, IDisposable
{
    [Inject] protected IDialogService DialogService { get; set; } = default!;
    [Inject] protected ISnackbar SnackbarFormBase { get; set; } = default!;
    [Inject] protected IHttpClientFactory HttpClientFactory { get; set; } = default!;
    [Inject] protected HotKeysHandle HotKeysHandle { get; set; } = default!;
    [Parameter] public string HttpClientName { get; set; } = default!;
    [Parameter] public string Endpoint { get; set; } = default!;
    [Parameter] public string? SuccessMessage { get; set; } = null;
    [Parameter] public string? FailedMessage { get; set; } = null;
    [Parameter] public bool DisableSuccessDefaultMessage { get; set; } = false;
    [Parameter] public bool DisableFailDefaultMessage { get; set; } = false;
    [Parameter] public string? ButtonSubmitText { get; set; } = null;

    protected CancellationTokenSource cancellationTokenSource = new();

    protected MudForm? form;
    protected HttpClient HttpClient { get { return HttpClientFactory.CreateClient(HttpClientName); } }
    protected HotKeysContext HotKeysContext = default!;


    protected async Task Submit()

    {
        await form!.Validate();

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
                    await Reset();
                    await Success(response);
                    break;
                case System.Net.HttpStatusCode.Unauthorized:
                case System.Net.HttpStatusCode.Forbidden:
                    SnackbarFormBase.Add(
                        message: "User doesn't have permission.",
                        severity: Severity.Error);
                    break;
                case System.Net.HttpStatusCode.InternalServerError:
                    SnackbarFormBase.Add(
                        message: "An unexpected error has occurred.",
                        severity: Severity.Error);
                    break;

                case System.Net.HttpStatusCode.BadRequest:
                default:
                    await Fail(response);
                    break;
            }
        }
        catch (TaskCanceledException ex) when (ex.CancellationToken.IsCancellationRequested)
        {
            SnackbarFormBase.Add("User canceled request!", MudBlazor.Severity.Warning);
        }
        catch (TaskCanceledException)
        {
            SnackbarFormBase.Add("Request timed out", MudBlazor.Severity.Warning);
        }
        catch (Exception ex)
        {
            SnackbarFormBase.Add($"Unexpected error:<BR/><BR>{ex.Message}", MudBlazor.Severity.Error);
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
            SnackbarFormBase.Add(
                message: FailedMessage,
                severity: Severity.Error);
        }
        else if (DisableFailDefaultMessage == false)
        {
            SnackbarFormBase.Add(
                message: FailedMessage ?? string.Join("<br/>", bad?.errors?.GeneralErrors!),
                severity: Severity.Error);
        }
    }

    protected void ShowSuccesMessage()
    {
        if (SuccessMessage is not null)
        {
            SnackbarFormBase.Add(
                message: SuccessMessage,
                severity: Severity.Success);
        }
        else if (DisableFailDefaultMessage == false)
        {
            SnackbarFormBase.Add(
                message: "Completed successfully",
                severity: Severity.Success);
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
    protected virtual async Task Reset()
    {
        form?.Reset();
        await Task.CompletedTask;
    }
    private void ResetCancelationToken()
    {
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
        cancellationTokenSource = new CancellationTokenSource();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        HotKeysContext = HotKeysHandle.Add()
            .Add(ModKeys.Ctrl, Keys.Enter, Submit, ButtonSubmitText, exclude: Exclude.InputNonText | Exclude.TextArea | Exclude.InputNonText);

    }
    public void Dispose()
    {
        HotKeysHandle.Remove();
    }
}
