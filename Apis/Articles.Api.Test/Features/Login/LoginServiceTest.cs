using Articles.Models.Feature.Login;
using System.Linq;

namespace Articles.Api.Test.Features.Login;
public class LoginServiceTest
    : IClassFixture<LoginServiceServiceCollectionFixture>
{
    private readonly ILoginService _loginService;

    public LoginServiceTest(LoginServiceServiceCollectionFixture spFixture)
    {
        _loginService = spFixture.ServiceProvider.GetRequiredService<ILoginService>();
    }

    [Fact]
    public async Task LoginSuccess()
    {
        //arrange
        var req = new UserLoginRequest
        {
            Email = "admin.test@article.ie",
            Password = "1234"
        };

        // act
        var response = await _loginService.Login(req, default);

        // assert
        response.FullName.ShouldBe("Admin Test");
        response.UserRoles.Count().ShouldBe(1);
        response.UserClaims.Count().ShouldBe(4);
    }

    [Fact]
    public async Task LoginInvalidInput()
    {
        //arrange
        var req = new UserLoginRequest();

        // act
        var response = await _loginService.Login(req, default);

        // assert
        response.ShouldBeOfType<NullUserLoginResponse>();
    }
}