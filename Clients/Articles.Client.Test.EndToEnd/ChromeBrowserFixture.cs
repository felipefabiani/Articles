using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Articles.Client.Test.EndToEnd
{
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
            Playwright1?.Dispose();
            Browser?.DisposeAsync();
        }
    }
}
