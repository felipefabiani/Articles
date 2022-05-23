namespace Articles.Client.Test.Specs.Features.Pages.Anonymous.Login;

[Binding, Scope(Tag = "Login")]
public class LoginStepDefinitions : IAsyncDisposable, IDisposable
{
    private LoginPageObject _page;

    [Given(@"a logged out user")]
    public async Task GivenALoggedOutUser()
    {
        _page = await LoginPageObject.Create<LoginPageObject>();
    }

    [When(@"the user attempts to log in with invalid email and password credentials")]
    public async Task WhenTheUserAttemptsToLogInWithInvalidEmailAndPasswordCredentials(Table table)
    {
        _page.SetData(table);
        await SetUser();
        await _page.ClickLoginButton();
    }

    [Then(@"the log is not logged in")]
    public void ThenTheLogIsNotLoggedIn()
    {
        _page.Page.Url.ShouldBe(_page.GetUrl());
    }

    [Then(@"the user messages for the inputs fields")]
    public async Task ThenTheUserMessagesForTheInputsFields()
    {
        (await _page.Page
            .Locator($"text={_page.Data.EmailMessage}")
            .CountAsync()
        ).ShouldBe(1);
        (await _page.Page
            .Locator($"text={_page.Data.PwdMessage}")
            .CountAsync()
        ).ShouldBe(1);
    }

    [When(@"the user attempts to log in with invalid credentials")]
    public async Task WhenTheUserAttemptsToLogInWithInvalidCredentials(Table table)
    {
        _page.SetData(table);
        await SetUser();
        await _page.Page.RunAndWaitForRequestFinishedAsync(async () =>
        {
            await _page.ClickLoginButton();
        });
    }

    [Then(@"the user see a error message")]
    public async Task ThenTheUserSeeAErrorMessage()
    {
        var alert = await _page.Page
             .Locator($"text={_page.Data.Alert}")
             .CountAsync();
        alert.ShouldBe(1);
    }

    [When(@"the user attempts to log in with valid credentials")]
    public async Task WhenTheUserAttemptsToLogInWithValidCredentials(Table table)
    {
        _page.SetData(table);
        await SetUser();
        await _page.Page.RunAndWaitForNavigationAsync(async () =>
        {
            await _page.ClickLoginButton();
        });
    }

    [Then(@"the log in successfully")]
    public void ThenTheLogInSuccessfully()
    {
        _page.Page.Url.ShouldNotBe(_page.GetUrl());
    }

    private async Task SetUser()
    {
        await _page.SetEmail(_page.Data?.Email);
        await _page.SetPassword(_page.Data?.Password);
    }

    public async ValueTask DisposeAsync()
    {
        await _page.DisposeAsync().ConfigureAwait(false);
    }

    public async void Dispose()
    {
        await _page.DisposeAsync().ConfigureAwait(false);
    }
}
