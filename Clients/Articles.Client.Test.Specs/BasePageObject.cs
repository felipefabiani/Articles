using Microsoft.Playwright;
using SpecFlow.Actions.Playwright;

namespace Articles.Client.Test.Specs;
public abstract class BasePageObject : IDisposable
{
    public virtual string ResetButtonSelector => "#reset";

    public virtual string SubmitButtonSelector => "#submit";
    public virtual string BaseAddress => "https://localhost:7202";
    public abstract string PagePath { get; }

    public Task<IPage> Page { get; init; }
    public Interactions Interactions { get; init; }
    public BrowserDriver Browser { get; set; }
    public string GetUrl() => $"{BaseAddress.Trim('/')}/{PagePath.Trim('/')}";

    public BasePageObject(BrowserDriver browser)
    {
        Browser = browser;
        Page = CreatePageAsync(browser.Current);
        Interactions = new Interactions(Page);
    }

    private async Task<IPage> CreatePageAsync(Task<IBrowser> browser) => await (await browser).NewPageAsync();

    public async Task EnsureCalculatorIsOpenAndResetAsync()
    {
        if ((await Page).Url != GetUrl())
        {
            await Interactions.GoToUrl(GetUrl());
        }
        else
        {
            await Interactions.ClickAsync(ResetButtonSelector);
        }
    }

    public void Dispose()
    {
        Page.Dispose();
        Browser.Dispose();
    }
}
