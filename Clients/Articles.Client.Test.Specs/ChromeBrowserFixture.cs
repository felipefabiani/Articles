using Microsoft.Playwright;

namespace Articles.Client.Test.Specs;

public class ChromeBrowserFixture : IDisposable
{
    public IPlaywright Playwright1 { get; set; }
    public IBrowser Browser { get; set; }
    public IPage Page { get; set; }


    public async Task InitializeAsync()
    {
        Playwright1 = await Playwright.CreateAsync();
        Browser = await Playwright1.Chromium.LaunchAsync();
        Page = await Browser.NewPageAsync();
        await Page.GotoAsync("https://localhost");
    }
    public void Dispose()
    {
        _ = Browser?.DisposeAsync();
        Playwright1?.Dispose();
    }
}
