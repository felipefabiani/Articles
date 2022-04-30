using Articles.Models.Feature.Login;

namespace Articles.Api.Features.Login;

public class LoginEndpoint : EndpointWithMapping<UserLoginRequest, UserLoginResponse, UserEntity>
{
    private readonly ILoginService _service;
    private readonly ILogger<LoginEndpoint> _log;

    public LoginEndpoint(
        ILoginService service,
        ILogger<LoginEndpoint> log)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _log = log ?? throw new ArgumentNullException(nameof(log));
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
