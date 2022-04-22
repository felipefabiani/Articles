using Articles.Client.Test.EndToEnd;
using SpecFlow.Actions.Playwright;

namespace Articles.Client.Test.Specs.Features.Pages.Anonymous.Login;
public class LoginPageObject : BasePageObject
{
    public override string PagePath => "login";

    public LoginPageObject(BrowserDriver browser)
        : base(browser)
    {
    }

    public readonly string EmailInputSelector = "#login-email";
    public readonly string PasswordInputSelector = "#login-password";

    // public Task SetEmail(string? email) => Interactions.SendTextAsync(EmailInputSelector, email ?? string.Empty);
    public Task SetEmail(string? email) => ClearAndSendTextAsync(EmailInputSelector, email ?? string.Empty);
    public Task SetPassword(string? pwd) => ClearAndSendTextAsync(PasswordInputSelector, pwd ?? string.Empty);
    public Task ClickLoginButton() => Interactions.ClickAsync(SubmitButtonSelector);
    public Task ClickResetButton() => Interactions.ClickAsync(ResetButtonSelector);

    private async Task ClearAndSendTextAsync(string selector, string email)
    {
        var p = await Page;
        await p.FillAsync(selector, string.Empty);
        await p.FillAsync(selector, email);
    }
}
