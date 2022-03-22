namespace Articles.Api.Features.Login;

public class LogingEndpoint : EndpointWithMapping<UserLoginRequest, UserLoginResponse, User>
{
    private readonly ILogingService _service;
    private readonly ILogger<LogingEndpoint> _log;

    public LogingEndpoint(
        ILogingService service,
        ILogger<LogingEndpoint> log)
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
