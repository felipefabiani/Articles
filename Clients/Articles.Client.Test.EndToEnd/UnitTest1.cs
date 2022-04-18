using Microsoft.Playwright;
using System.Threading.Tasks;
using Xunit;

namespace Articles.Client.Test.EndToEnd
{
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

        }
    }
}