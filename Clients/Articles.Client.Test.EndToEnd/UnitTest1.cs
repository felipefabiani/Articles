using Microsoft.Playwright;
using System.Threading.Tasks;
using Xunit;

namespace Articles.Client.Test.EndToEnd
{
<<<<<<< HEAD
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://playwright.dev/dotnet");
            await page.ScreenshotAsync(new PageScreenshotOptions { Path = "screenshot.png" });
=======
    public class UnitTest1 :
        IClassFixture<ChromeBrowserFixture>
    {
        private readonly ChromeBrowserFixture _cbf;

        public UnitTest1(ChromeBrowserFixture cbf)
        {
            _cbf = cbf;
            _cbf.InitializeAsync();
        }
        [Fact]
        public async Task Test1()
        {   
            //using (_cbf)
            //{
            //    await page.GotoAsync("https://playwright.dev/dotnet");
            //await page.ScreenshotAsync(new PageScreenshotOptions { Path = "screenshot1.png" });
            //}

>>>>>>> Add project files.
        }
    }
}