using Articles.Client.Pages.Anonymous;
using Articles.Client.Test.EndToEnd;
using Articles.Models.Feature.Login;
using AutoFixture;
using Bunit;
using MudBlazor;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Articles.Client.Test.Pages.Anonymous;

public class LoginTest : IDisposable
// : IClassFixture<TestContextFixture>
{
    private readonly TestContextFixture _ctxFixture;
    private readonly Fixture _fixture;

    public LoginTest()
    {
        _ctxFixture = new TestContextFixture();
        _fixture = new Fixture();
    }

    public void Dispose()
    {
        _ctxFixture.Dispose();
    }

    [Theory]
    [InlineData(null, null, "Username is required!", "Password is required!")]
    [InlineData("", "", "Username is required!", "Password is required!")]
    [InlineData("test", "123", "Invalid email format!", "Password length must be between 6 and 10 characters!")]
    [InlineData("test.cscs", "01234567891", "Invalid email format!", "Password length must be between 6 and 10 characters!")]
    public void InvalidInputs(
        string? email,
        string? password,
        string emailMessage,
        string pwdMessage)
    {
        // Arrange
        _ctxFixture.AddHttpClient(new UserLoginResponse
        {
            FullName = "Admin Test"
        });

        var cut = _ctxFixture.Context.RenderComponent<Login>();

        // Act
        cut.Find("input#login-email").Change(email);
        cut.Find("input#login-password").Change(password);
        cut.Find("button#submit").Click();

        // Assert
        var validators = cut.FindAll("p.mud-input-helper-text.mud-input-error");
        validators.Count.ShouldBe(2);
        cut.Markup.ShouldContain(emailMessage);
        cut.Markup.ShouldContain(pwdMessage);
    }

    [Theory]
    [InlineData("test@t", "012345")]
    [InlineData("test@t.com", "0123456789")]
    public async void ValidInputsInvalidUser(
    string email,
    string password)
    {
        // Arrange
        _ctxFixture.AddHttpClient(
           response: new BadRequestResponse
        {
            statusCode = 400,
            message = "One or more errors occured!",
            errors = new Errors
            {
                GeneralErrors = new string[]
                {
                    "User or password incorrect!"
                }
            }

        }, 
          statusCode: HttpStatusCode.BadRequest, 
          endpoint: "api/login");

        var cut = _ctxFixture.Context.RenderComponent<Login>();

        // Act
        cut.Find("input#login-email").Change(email);
        cut.Find("input#login-password").Change(password);
        await cut.Find("button#submit").ClickAsync(null!);

        // Assert
        var alert = cut.FindComponents<MudAlert>();

        alert.Count.ShouldBe(1);
        alert[0].Markup.ShouldContain("User or password incorrect!");
    }

    [Theory]
    [InlineData("test@t", "012345")]
    [InlineData("test@t.com", "0123456789")]
    public async void ValidInputsValidUser(
        string email,
        string password)
    {
        // Arrange
        var ret = _fixture.Create<UserLoginResponse>();
        _ctxFixture.AddHttpClient(
            response: ret,
            statusCode: HttpStatusCode.OK,
            endpoint: "api/login");

        var cut = _ctxFixture.Context.RenderComponent<Login>();

        // Act
        cut.Find("input#login-email").Change(email);
        cut.Find("input#login-password").Change(password);
        await cut.Find("button#submit").ClickAsync(null!);

        // Assert
        //var alert = cut.FindComponents<MudAlert>();

        //alert.Count.ShouldBe(1);
        //alert[0].Markup.ShouldContain("User or password incorrect!");
    }
}
