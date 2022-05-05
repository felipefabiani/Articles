namespace Articles.Api.Test.Features.Articles.SaveArticle;

public class SaveArticleEndpointTester
{

    //[Fact]
    //public async Task AdminLoginSuccess()
    //{
    //    //arrange
    //    var fixture = new Fixture();
    //    var req = fixture
    //        .Build<SaveArticleRequest>()
    //        .OmitAutoProperties()
    //        .Create();



    //    var logingService = A.Fake<ILoginService>();
    //    var logger = A.Fake<ILogger<LoginEndpoint>>();
    //    _ = A.CallTo(() => logingService.Login(req, default)).Returns(Task.FromResult(new UserLoginResponse
    //    {
    //        FullName = "Test",
    //        Token = new JwtToken
    //        {
    //            ExpiryDate = DateTime.UtcNow.AddHours(4),
    //            Value = JWTBearer.CreateToken(
    //                signingKey: new Guid().ToString(),
    //                expireAt: DateTime.UtcNow.AddHours(4),
    //                roles: new[] { "admin" },
    //                claims: new[] { ("admin", "100") }
    //            )
    //        }
    //    }));

    //    var ep = Factory.Create<LoginEndpoint>(logingService, logger);

    //    //act
    //    await ep.HandleAsync(req, default);
    //    var rsp = ep.Response;

    //    ////assert
    //    rsp.HasToken.ShouldBeTrue();
    //    rsp.FullName.ShouldBe("Test");
    //}

    //[Fact]
    //public async Task AdminLoginInvalidInput()
    //{
    //    //arrange
    //    var req = new UserLoginRequest();

    //    var logingService = A.Fake<ILoginService>();
    //    A.CallTo(() => logingService.Login(req, default))
    //        .Returns(Task.FromResult((UserLoginResponse)NullUserLoginResponse.Empty));

    //    var ep = Factory.Create<LoginEndpoint>(
    //        logingService,
    //        A.Fake<ILogger<LoginEndpoint>>());

    //    var ex = await Should.ThrowAsync<ValidationFailureException>(async () => await ep.HandleAsync(req, default));
    //}
}