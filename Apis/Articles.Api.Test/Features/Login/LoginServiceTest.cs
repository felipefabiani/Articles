using Articles.Models.Feature.Login;
using Articles.Test.Helper.Bases;
using Articles.Test.Helper.Fixture;
using System.Linq;

namespace Articles.Api.Test.Features.Login;
public class LoginServiceTest :
    ContextDb<ServiceCollectionFixture>
{
    private readonly ILoginService _loginService;

    public LoginServiceTest(
        ServiceCollectionFixture spFixture) :
        base(spFixture)
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
            Password = "123456"
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