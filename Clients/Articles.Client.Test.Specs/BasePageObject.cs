using Articles.Helper.Auth;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Playwright;
using System.IdentityModel.Tokens.Jwt;
using TechTalk.SpecFlow.Assist;
using static Articles.Helper.ArticlesConstants.Security;

namespace Articles.Client.Test.Specs;

public abstract partial class BasePageObject<TData> : IAsyncDisposable
    where TData : class
{
    private static string? _token;

    public virtual string ResetButtonSelector => "#reset";
    public virtual string SubmitButtonSelector => "#submit";
    public virtual string BaseAddress => "https://localhost:665";
    public abstract string PagePath { get; }
    public IPage Page { get; private set; }
    public IPlaywright Playwright1 { get; private set; }
    public IBrowser Browser { get; private set; }
    public IBrowserContext Context { get; private set; }

    public TData Data { get; private set; }
    public string GetUrl() => $"{BaseAddress.Trim('/')}/{PagePath.Trim('/')}";

    protected BasePageObject()
    {
    }

    public async Task Login(string token)
    {
        ValidateToken(token);

        await Page
            .EvaluateAsync<string>(
                "(token) => window.localStorage.setItem('authToken',token)",
                new[] { token })
            .ConfigureAwait(false);
        await Page.ReloadAsync(new PageReloadOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle
        });
        await WaitPageFullyLoad().ConfigureAwait(false);
    }
    public static async Task<TPage> Create<TPage>(string? token = null, bool gotoPage = true)
        where TPage : BasePageObject<TData>, new()
    {
        var bp = new TPage();

        await bp.CreatePageAsync().ConfigureAwait(false);

        _token = token;

        await bp.EnsureIsOpenAndResetAsync(gotoPage).ConfigureAwait(false);
        return bp;
    }
    public async Task EnsureIsOpenAndResetAsync(bool gotoPage)
    {
        if (Page.Url != GetUrl())
        {
            await Page.GotoAsync(BaseAddress).ConfigureAwait(false);
            await WaitPageFullyLoad().ConfigureAwait(false);

            if (!string.IsNullOrEmpty(_token))
            {
                await Login(_token).ConfigureAwait(false);
            }

            if (gotoPage)
            {
                await GotoPage().ConfigureAwait(false);
            }
        }
        else
        {
            await Page.ClickAsync(ResetButtonSelector).ConfigureAwait(false);
        }
        await WaitPageFullyLoad().ConfigureAwait(false);
    }

    public async Task GotoPage()
    {
        await Page.RunAndWaitForNavigationAsync(async () =>
        {
            await Page.GotoAsync(
                url: GetUrl(),
                options: new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });
        }, new PageRunAndWaitForNavigationOptions
        {
            UrlString = $"**/{PagePath}",
            WaitUntil = WaitUntilState.DOMContentLoaded
        }).ConfigureAwait(false);

        await WaitPageFullyLoad().ConfigureAwait(false);
    }

    public async Task WaitPageFullyLoad()
    {
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle).ConfigureAwait(false);
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded).ConfigureAwait(false);
        await Page.WaitForLoadStateAsync(LoadState.Load).ConfigureAwait(false);
    }

    public void SetData(Table table)
    {
        Data = table.CreateInstance<TData>(
            new InstanceCreationOptions { VerifyAllColumnsBound = true });
    }
    public async ValueTask DisposeAsync()
    {
        if (Context is not null)
        {
            await Context.DisposeAsync().ConfigureAwait(false);
        }

        if (Browser is not null)
        {
            await Browser.DisposeAsync().ConfigureAwait(false);
        }

        Playwright1?.Dispose();

        Context = null!;
        Browser = null!;
        Playwright1 = null!;
    }
    private static void ValidateToken(string token)
    {
        var issuerSigningCertificate = new SigningIssuerCertificate();
        var validationParameters = new TokenValidationParameters
        {
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = issuerSigningCertificate.GetIssuerSigningKey(),
            ValidateAudience = true,
            ValidAudience = Audience,
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var handler = new JwtSecurityTokenHandler();
            handler.OutboundClaimTypeMap.Clear();
            handler.ValidateToken(token, validationParameters, out var validatedToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Invalid token, to login provide a valide token using CreateToken from Articles.Helper.Extensions", ex);
        }
    }
    private async Task CreatePageAsync()
    {
        Playwright1 = await Playwright.CreateAsync();

        Browser = await Playwright1.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
#if DEBUG
            Headless = false,
            SlowMo = 0,
            Devtools = false
#else
            Headless = false,
            SlowMo = 0,
            Devtools = false
#endif
        }).ConfigureAwait(false);
        Context = await Browser!.NewContextAsync().ConfigureAwait(false);
        Page = await Context.NewPageAsync().ConfigureAwait(false);
    }
}

public abstract partial class BasePageObject<TData>
{
    protected async Task ClearAndSendTextAsync(string selector, string text)
    {
        await Page.FillAsync(selector, string.Empty).ConfigureAwait(false);
        await Page.FillAsync(selector, text).ConfigureAwait(false);
    }
    protected async Task PressTabAsync(string locator) =>
        await Page.Locator(locator).PressAsync("Tab").ConfigureAwait(false);

    public async Task<bool> HasSuccessSnack(string message) => await Page
        .Locator($@"div.mud-alert-filled-success > div:has-text(""{message}"")")
        .CountAsync()
        .ConfigureAwait(false) == 1;

    public async Task<bool> HasNotAutorizedAccess() => await Page
        .Locator(@"div.mud-main-content > p[role=alert]:has-text(""You are not authorized to access this resource."")")
        .CountAsync()
        .ConfigureAwait(false) == 1;

}