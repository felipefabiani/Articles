using Articles.Models.Auth;
using Articles.Models.Feature.Login;
using Articles.Test.Helper.Bases;
using Articles.Test.Helper.Fixture;
using Microsoft.AspNetCore.Http;

namespace Articles.Api.Test.Features.Login;
public class LoginEndpointTest :
    ServiceProvider<ServiceCollectionFixture>
{
    private readonly DefaultHttpContext _defaultHttpContext;

    public LoginEndpointTest(
        ServiceCollectionFixture spFixture) :
        base(spFixture)
    {
        _defaultHttpContext = spFixture.ServiceProvider.GetRequiredService<DefaultHttpContext>();
    }

    [Fact]
    public async Task AdminLoginSuccess()
    {
        //arrange
        var req = new UserLoginRequest
        {
            Email = "admin",
            Password = "pass"
        };

        var logingService = A.Fake<ILoginService>();
        _ = A
            .CallTo(() => logingService.Login(req, default))
            .Returns(Task.FromResult(new UserLoginResponse
            {
                FullName = "Test",
                Token = new JwtToken
                {
                    ExpiryDate = DateTime.UtcNow.AddHours(4),
                    Value = JWTBearer.CreateToken(
                        signingKey: new Guid().ToString(),
                        expireAt: DateTime.UtcNow.AddHours(4),
                        roles: new[] { "admin" },
                        claims: new[] { ("admin", "100") }
                    )
                }
            }));

        var ep = Factory.Create<LoginEndpoint>(_defaultHttpContext, new object[] { logingService });

        //act
        await ep.HandleAsync(req, default);
        var rsp = ep.Response;

        ////assert
        rsp.HasToken.ShouldBeTrue();
        rsp.FullName.ShouldBe("Test");
    }

    [Fact]
    public async Task AdminLoginInvalidInput()
    {
        //arrange
        var req = new UserLoginRequest();

        var logingService = A.Fake<ILoginService>();
        _ = A
            .CallTo(() => logingService.Login(req, default))
            .Returns(Task.FromResult((UserLoginResponse)NullUserLoginResponse.Empty));

        var ep = Factory.Create<LoginEndpoint>(_defaultHttpContext, new object[] { logingService });
        var ex = await Should.ThrowAsync<ValidationFailureException>(async () => await ep.HandleAsync(req, default));
    }
}
