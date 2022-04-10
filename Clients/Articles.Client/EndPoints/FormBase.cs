using Articles.Client.Pages;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Articles.Client.EndPoints;
public class FormBase<TRequest, TResponse> : ComponentBase
    where TRequest : class, new()
    where TResponse : notnull, new()
{
    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IServiceProvider ServiceProvider { get; set; } = null!;
    [Inject] private ArticleApiClient Client { get; set; } = null!;

    [Parameter] public IPostEndPoint<TRequest, TResponse> Endpoint { get; set; } = null!;
    [Parameter] public TRequest Model { get; set; } = new TRequest();
    [Parameter] public RenderFragment HeaderTemplate { get; set; } = default!;
    [Parameter] public RenderFragment<TRequest> FormTemplate { get; set; } = default!;
    [Parameter] public RenderFragment ButtonsTemplate { get; set; } = default!;
    [Parameter] public string MessageOnFormValid { get; set; } = string.Empty;
    [Parameter] public string MessageOnFormInvalid { get; set; } = string.Empty;
    [Parameter] public Action<TResponse>? CallBack { get; set; }

    protected CancellationTokenSource cancellationTokenSource = new();
    protected AbstractValidator<TRequest> _validator = null!;

    protected MudForm form = null!;
    protected async Task Submit()
    {
        await form.Validate();

        if (form.IsValid)
        {
            var dlgRef = DialogService.Show<CancelDialog>("",
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

            try
            {
                TResponse resp = await Client.PostAsync<TRequest, TResponse>(Endpoint, cancellationTokenSource.Token);
                CallBack?.Invoke(resp);
            }
            catch (TaskCanceledException ex) when (ex.CancellationToken.IsCancellationRequested)
            {
                Snackbar.Add("User canceled request", MudBlazor.Severity.Warning);
                ResetCancelationToken();
            }
            catch (TaskCanceledException)
            {
                Snackbar.Add("Request timed out", MudBlazor.Severity.Warning);
                ResetCancelationToken();
            }
            finally
            {
                dlgRef.Close();
            }
        }
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
        if (ServiceProvider.GetService(typeof(AbstractValidator<TRequest>)) is AbstractValidator<TRequest> validator)
        {
            _validator = validator;
        }

        Endpoint.Model = Model.CloneJson();
        return base.OnInitializedAsync();
    }

    public Func<object, string, IEnumerable<string>> ValidateValue => (mod, propertyName) =>
    {
        var result = _validator.Validate(ValidationContext<TRequest>.CreateWithOptions((TRequest)mod, x => x.IncludeProperties(propertyName)));
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
