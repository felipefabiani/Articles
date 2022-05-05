using Articles.Models.Feature.Login;

namespace Articles.Api.Features.Login;

public class LoginEndpoint : EndpointWithMapping<UserLoginRequest, UserLoginResponse, UserEntity>
{
    private readonly ILoginService _service;

    public LoginEndpoint(ILoginService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }
    public override void Configure()
    {
        Post("/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UserLoginRequest r, CancellationToken c)
    {
        Response = await _service.Login(r, c);

        // await Task.Delay(10_000, c);

        if (!Response.HasToken)
        {
            ThrowError("User or password incorrect!");
        }

        await SendAsync(Response, cancellation: c);
    }
}
