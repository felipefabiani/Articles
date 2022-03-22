namespace Articles.Api.Test.Features.Login;
public class LoginEndpoint
{
    [Fact]
    public async Task AdminLoginSuccess()
    {
        //arrange
        var fakeConfig = A.Fake<IConfiguration>();
        A.CallTo(() => fakeConfig["TokenKey"]).Returns("0000000000000000");

        //var ep = Factory.Create<AdminLogin>(
        //    A.Fake<ILogger<AdminLogin>>(), //mock dependencies for injecting to the constructor
        //    A.Fake<IEmailService>(),
        //    fakeConfig);

        //var req = new AdminLoginRequest
        //{
        //    UserName = "admin",
        //    Password = "pass"
        //};

        ////act
        //await ep.HandleAsync(req, default);
        //var rsp = ep.Response;

        ////assert

        //Assert.IsNotNull(rsp);
        //Assert.IsFalse(ep.ValidationFailed);
        //Assert.IsTrue(rsp.Permissions.Contains("Inventory_Delete_Item"));
    }
}
