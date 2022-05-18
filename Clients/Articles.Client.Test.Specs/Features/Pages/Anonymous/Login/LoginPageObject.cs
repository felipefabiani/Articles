namespace Articles.Client.Test.Specs.Features.Pages.Anonymous.Login;
public class LoginPageObject : BasePageObject<LoginPageObject.User>
{
    public override string PagePath => "login";

    public LoginPageObject()
    {
    }

    public readonly string EmailInputSelector = "#login-email";
    public readonly string PasswordInputSelector = "#login-password";

    // public Task SetEmail(string? email) => Interactions.SendTextAsync(EmailInputSelector, email ?? string.Empty);
    public Task SetEmail(string? email) => ClearAndSendTextAsync(EmailInputSelector, email ?? string.Empty);
    public Task SetPassword(string? pwd) => ClearAndSendTextAsync(PasswordInputSelector, pwd ?? string.Empty);
    public Task ClickLoginButton() => Page.ClickAsync(SubmitButtonSelector);
    public Task ClickResetButton() => Page.ClickAsync(ResetButtonSelector);

    public record User(
        string? Email = null,
        string? Password = null,
        string? EmailMessage = null,
        string? PwdMessage = null,
        string? Alert = null);
}
