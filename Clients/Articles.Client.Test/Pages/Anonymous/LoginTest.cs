using Articles.Client.Pages.Anonymous;
using Articles.Client.Test.EndToEnd;
<<<<<<< HEAD
using Articles.Models.Feature.Login;
using AutoFixture;
using Bunit;
=======
using Articles.Models.Auth;
using Articles.Models.Feature.Login;
using AutoFixture;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
>>>>>>> Add Test for blazor components.
using MudBlazor;
using Shouldly;
using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Net;
using Xunit;
=======
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Xunit;
using ssc = System.Security.Claims;
>>>>>>> Add Test for blazor components.

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
    [InlineData("test", "123", "Please include an '@' in the email address.", "Password length must be between 6 and 10 characters.")]
    [InlineData("test.cscs", "01234567891", "Please include an '@' in the email address.", "Password length must be between 6 and 10 characters.")]
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

        var decodedHtml = HttpUtility.HtmlDecode(cut.Markup);
        decodedHtml.ShouldContain(emailMessage);
        decodedHtml.ShouldContain(pwdMessage);
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
<<<<<<< HEAD
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
=======
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
>>>>>>> Add Test for blazor components.
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
<<<<<<< HEAD
        var ret = _fixture.Create<UserLoginResponse>();
        _ctxFixture.AddHttpClient(
            response: ret,
            statusCode: HttpStatusCode.OK,
            endpoint: "api/login");

        var cut = _ctxFixture.Context.RenderComponent<Login>();
=======
        var resp = new UserLoginResponse
        {
            FullName = "Admin Test",
            UserRoles = new List<string> { "admin" },
            UserClaims = new List<string> { "Test" },
            Token = new JwtToken
            {
                ExpiryDate = DateTime.UtcNow.AddHours(4),
                Value = CreateToken(
                    signingKey: Guid.NewGuid().ToString(),
                    expireAt: DateTime.Now.AddHours(4),
                    roles: new List<string> { "Admin" },
                    claims: new List<ssc.Claim> { new ssc.Claim("Article_Moderate", "100") },
                    firstName: "Admin",
                    lastName: "Test")
                }
        };
        _ctxFixture.AddHttpClient(
            response: resp,
            statusCode: HttpStatusCode.OK,
            endpoint: "api/login");

        var returnUrl = "counter";
        var cut = _ctxFixture.Context.RenderComponent<Login>(parameters =>
            parameters.Add(p => p.ReturnUrl, returnUrl));

        var navMan = _ctxFixture.Context.Services.GetRequiredService<FakeNavigationManager>();
        navMan.NavigateTo("http://localhost/api/login");
>>>>>>> Add Test for blazor components.

        // Act
        cut.Find("input#login-email").Change(email);
        cut.Find("input#login-password").Change(password);
        await cut.Find("button#submit").ClickAsync(null!);

        // Assert
<<<<<<< HEAD
        //var alert = cut.FindComponents<MudAlert>();

        //alert.Count.ShouldBe(1);
        //alert[0].Markup.ShouldContain("User or password incorrect!");
=======
        navMan.Uri.ShouldBe($"{navMan.BaseUri}{returnUrl}");
    }

    public string CreateToken(
        string signingKey, 
        DateTime? expireAt = null, 
        IEnumerable<string>? permissions = null, 
        IEnumerable<string>? roles = null, 
        IEnumerable<ssc.Claim>? claims = null,
        string? firstName = null,
        string? lastName = null)
    {
        var list = new List<ssc.Claim>();
        if (claims != null)
        {
            list.AddRange(claims);
            list.Add(new ssc.Claim(ssc.ClaimTypes.Name, $"{firstName} {lastName}"));
        }

        if (permissions != null)
        {
            list.AddRange(permissions.Select((string p) => new ssc.Claim("permissions", p)));
        }

        if (roles != null)
        {
            list.AddRange(roles.Select((string r) => new ssc.Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", r)));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ssc.ClaimsIdentity(list),
            Expires = expireAt,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signingKey)), "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256")
        };
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        jwtSecurityTokenHandler.OutboundClaimTypeMap.Clear();
        return jwtSecurityTokenHandler.WriteToken(jwtSecurityTokenHandler.CreateToken(tokenDescriptor));
>>>>>>> Add Test for blazor components.
    }
}
