using Articles.Client.Test.Specs.Features.Pages.Anonymous.Login;

namespace Articles.Client.Test.Specs.Hooks;

[Binding]
public class TestHooks
{
    [BeforeScenario("Login")]
    public async Task BeforeLoginScenario(LoginPageObject loginPage)
    {
        await loginPage.EnsureCalculatorIsOpenAndResetAsync();
    }

    [AfterScenario]
    public async Task AfterScenario(LoginPageObject loginPage)
    {
        loginPage.Dispose();

        await Task.CompletedTask;
    }
}
