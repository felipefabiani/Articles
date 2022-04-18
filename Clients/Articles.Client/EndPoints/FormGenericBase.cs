using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Articles.Client.EndPoints;
public abstract class FormBase<TRequest, TResponse> : FormBase
    where TRequest : class, new()
    where TResponse : notnull, new()
{
    [Inject] protected AbstractValidator<TRequest> Validator { get; set; } = null!;
    [Parameter] public TRequest DefaultModel { get; set; } = new ();
    [Parameter] public RenderFragment HeaderTemplate { get; set; } = default!;
    [Parameter] public RenderFragment<TRequest> FormTemplate { get; set; } = default!;
    [Parameter] public RenderFragment ButtonsTemplate { get; set; } = default!;
    [Parameter] public Action<TResponse>? SuccessCallBack { get; set; }
    [Parameter] public Action<BadRequestResponse>? FailCallBack { get; set; }

    protected TRequest _model = new ();

    protected override async Task Fail(HttpResponseMessage response)
    {
        var bad = await response.Content.ReadFromJsonAsync<BadRequestResponse>();

        ShowFailMessage(bad);

        FailCallBack?.Invoke(bad);
    }

    protected override async Task Success(HttpResponseMessage response)
    {
        var result = await response.Content.ReadFromJsonAsync<TResponse>();

        ShowSuccesMessage();

        SuccessCallBack?.Invoke(result);
    }

    protected override Task Reset()
    {
        base.Reset();
        _model = DefaultModel.CloneJson(); ;
        return Task.CompletedTask;
    }
    protected override Task OnInitializedAsync()
    {
        _model = DefaultModel.CloneJson();
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
}
