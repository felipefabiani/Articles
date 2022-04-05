namespace Articles.Api.Features.Login;

public class LoginEndpoint : EndpointWithMapping<UserLoginRequest, UserLoginResponse, User>
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

        if (!Response.HasToken)
        {
            ThrowError("User or password incorrect!");
        }

        await SendAsync(Response, cancellation: c);
    }
}
