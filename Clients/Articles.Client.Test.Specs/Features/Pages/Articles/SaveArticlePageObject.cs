using Articles.Helper.Extensions;
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

    // public Task SetEmail(string? email) => Interactions.SendTextAsync(EmailInputSelector, email ?? string.Empty);
    public Task SetTitle(string? email) => ClearAndSendTextAsync(TitleInputSelector, email ?? string.Empty);
    public Task SetContent(string? pwd) => ClearAndSendTextAsync(ContentInputSelector, pwd ?? string.Empty);
    public Task ClickAddButton() => Page.ClickAsync(SubmitButtonSelector);
    public Task ClickResetButton() => Page.ClickAsync(ResetButtonSelector);

    public record SaveArticle(
        string? Title,
        string? Content,
        string? TitleMessage,
        string? ContentMessage);
}