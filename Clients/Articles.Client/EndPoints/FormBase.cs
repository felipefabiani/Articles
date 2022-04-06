using Articles.Client.Properties.EndPoints;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Articles.Client.EndPoints;
public class FormBase<TRequest, TResponse> : ComponentBase
    where TRequest : class, new()
    where TResponse : notnull, new()
{
    [Inject] ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IServiceProvider ServiceProvider { get; set; } = null!;
    [Inject] private ArticleApiClient Client { get; set; } = null!;

    [Parameter] public IPostEndPoint<TRequest, TResponse> Endpoint { get; set; } = null!;
    [Parameter] public TRequest Model { get; set; } = new TRequest();
    [Parameter] public RenderFragment<TRequest> FormTemplate { get; set; } = default!;
    [Parameter] public RenderFragment ButtonsTemplate { get; set; } = default!;
    [Parameter] public string MessageOnFormValid { get; set; } = string.Empty;
    [Parameter] public string MessageOnFormInvalid { get; set; } = string.Empty;
    [Parameter] public Action<TResponse>? CallBack { get; set; }

    protected AbstractValidator<TRequest> _validator = null!;

    protected MudForm form = null!;
    protected async Task Submit()
    {
        await form.Validate();

        if (form.IsValid)
        {
            TResponse resp = await Client.PostAsync<TRequest, TResponse>(Endpoint);
            CallBack?.Invoke(resp);
            return;
        }
    }
    protected Task Reset()
    {
        form.Reset();
        Endpoint.Model = Model.CloneJson();;
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
}
