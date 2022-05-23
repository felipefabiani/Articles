using Articles.Helper.Extensions;
using Microsoft.Playwright;
using ssc = System.Security.Claims;

namespace Articles.Client.Test.Specs.Features.Pages.Articles;

public class SaveArticlePageObject : BasePageObject<SaveArticlePageObject.SaveArticle>
{
    public override string PagePath => "add-article";
    public static string AuthorToken => DateTime.UtcNow
        .AddHours(4).CreateToken(
            id: 3,
            userName: "Author Test Mock",
            roles: new[] { "Author" },
            claims: new[] { new ssc.Claim("Author_Save_Own", "201") });

    public static string NonAuthorToken => DateTime.UtcNow
        .AddHours(4).CreateToken(
            id: 3,
            userName: "User Mock",
            roles: new[] { "User" },
            claims: new[] { new ssc.Claim("User_Reads", "301") });

    public readonly string TitleInputSelector = "#article-title";
    public readonly string ContentInputSelector = "#article-content";
    public readonly string ArticleMenuSelector = "button.mud-nav-link:has-text(\"Article\")";
    public readonly string AddArticleMenuSelector = "a[href='/add-article'].mud-nav-link:has-text('Add Article')";

    // public Task SetEmail(string? email) => Interactions.SendTextAsync(EmailInputSelector, email ?? string.Empty);
    public Task SetTitle(string? email) => ClearAndSendTextAsync(TitleInputSelector, email ?? string.Empty);
    public Task SetContent(string? pwd) => ClearAndSendTextAsync(ContentInputSelector, pwd ?? string.Empty);
    public Task ClickAddButton() => Page.ClickAsync(SubmitButtonSelector);
    public Task ClickResetButton() => Page.ClickAsync(ResetButtonSelector);
    public async Task ClickArticleMenu()
    {
        if (!await HasArticleMenu())
        {
            return;
        }
        await Page.ClickAsync(ArticleMenuSelector);
    }
    public async Task ClickAddArticleMenu()
    {
        if (!await HasAddArticleMenu())
        {
            return;
        }

        
        await Page.RunAndWaitForNavigationAsync(
            async () => await Page.ClickAsync(AddArticleMenuSelector),
            new PageRunAndWaitForNavigationOptions
            {
                UrlString = $"**/{PagePath}",
                WaitUntil = WaitUntilState.NetworkIdle
            });
    }

    public async Task<bool> HasArticleMenu() => await Page.Locator(ArticleMenuSelector).CountAsync() == 1;
    public async Task<bool> HasAddArticleMenu() => await Page.Locator(AddArticleMenuSelector).CountAsync() == 1;
    public async Task<bool> IsPageLoad()
    {
        return
            Page.Url.Contains(PagePath) &&
            await Page.Locator(@"div.mud-card-header > h1:has-text('Add Article')").CountAsync() == 1;
    }

    public record SaveArticle(
        string? Title,
        string? Content,
        string? TitleMessage,
        string? ContentMessage);
}