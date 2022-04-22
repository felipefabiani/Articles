using Microsoft.Playwright;
using TechTalk.SpecFlow.Assist;

namespace Articles.Client.Test.Specs.Features.Pages.Anonymous.Login;

[Binding]
public class LoginStepDefinitions
{
    private readonly LoginPageObject _loginPage;
    private IPage _page = null!;
    private User _user = null!;

    public LoginStepDefinitions(LoginPageObject loginPage)
    {
        _loginPage = loginPage;
    }

    [Given(@"a logged out user")]
    public async Task GivenALoggedOutUser()
    {
        await _loginPage.EnsureCalculatorIsOpenAndResetAsync();
        _page = await _loginPage.Page;
    }

    [When(@"the user attempts to log in with invalid email and password credentials")]
    public async Task WhenTheUserAttemptsToLogInWithInvalidEmailAndPasswordCredentials(Table table)
    {
        await SetUser(table);
        await _loginPage.ClickLoginButton();
    }

    [Then(@"the log is not logged in")]
    public void ThenTheLogIsNotLoggedIn()
    {
        _page.Url.ShouldBe(_loginPage.GetUrl());
    }

    [Then(@"the user messages for the inputs fields")]
    public async Task ThenTheUserMessagesForTheInputsFields()
    {
        (await _page
            .Locator($"text={_user.EmailMessage}")
            .CountAsync()
        ).ShouldBe(1);
        (await _page
            .Locator($"text={_user.PwdMessage}")
            .CountAsync()
        ).ShouldBe(1);
    }

    [When(@"the user attempts to log in with invalid credentials")]
    public async Task WhenTheUserAttemptsToLogInWithInvalidCredentials(Table table)
    {
        await SetUser(table);
        await _page.RunAndWaitForRequestFinishedAsync(async () =>
        {
            await _loginPage.ClickLoginButton();
        });
    }

    [Then(@"the user see a error message")]
    public async Task ThenTheUserSeeAErrorMessage()
    {
        var alert = await _page
             .Locator($"text={_user.Alert}")
             .CountAsync();
        alert.ShouldBe(1);
    }

    [When(@"the user attempts to log in with valid credentials")]
    public async Task WhenTheUserAttemptsToLogInWithValidCredentials(Table table)
    {
        await SetUser(table);
        await _page.RunAndWaitForNavigationAsync(async () =>
        {
            await _loginPage.ClickLoginButton();
        });
    }

    [Then(@"the log in successfully")]
    public void ThenTheLogInSuccessfully()
    {
        _page.Url.ShouldNotBe(_loginPage.GetUrl());
    }

    private async Task SetUser(Table table)
    {
        _user = table.CreateInstance<User>(new InstanceCreationOptions { VerifyAllColumnsBound = true });
        await _loginPage.SetEmail(_user?.Email);
        await _loginPage.SetPassword(_user?.Password);
    }
}

public record User(
    string? Email,
    string? Password,
    string? EmailMessage,
    string? PwdMessage,
    string? Alert);