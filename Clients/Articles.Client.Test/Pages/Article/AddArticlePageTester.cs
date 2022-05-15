using Articles.Client.Pages.Article;
using Articles.Models.Feature.Articles.SaveArticle;
using Articles.Test.Helper.TheoryData;

namespace Articles.Client.Test.Pages.Article;

public class AddArticlePageTesterBase : IDisposable
{
    protected readonly TestContextFixture _ctxFixture;

    protected ISnackbar SnackbarService { get => _ctxFixture.Context.Services.GetRequiredService<ISnackbar>(); }

    public AddArticlePageTesterBase()
    {
        _ctxFixture = new TestContextFixture();
    }

    public void Dispose()
    {
        _ctxFixture.Dispose();
    }
}

public class AddArticlePageTester : AddArticlePageTesterBase
{

    [Theory]
    [ClassData(typeof(SaveArticleInvalidParamTheoryData))]
    public async Task InvalidInputs(SaveArticleTheoryModel data)
    {
        // Arrange
        _ctxFixture.AddHttpClient(
            response: new SaveArticleResponse
            {
                Id = 0
            },
            endpoint: "api/articles/save-article");

        var cut = _ctxFixture.Context.RenderComponent<AddArticlePage>();

        // Act
        cut.Find("input#article-title").Change(data.Title);
        cut.Find("textarea#article-content").Change(data.Content);
        await cut.Find("button#submit").ClickAsync(null!);

        // Assert
        var validators = cut.FindAll("p.mud-input-helper-text.mud-input-error");
        validators.Count.ShouldBe(2);

        var decodedHtml = HttpUtility.HtmlDecode(cut.Markup);
        decodedHtml.ShouldContain(data.TitleMessage);
        decodedHtml.ShouldContain(data.ContentMessage);
    }

    [Theory]
    [ClassData(typeof(SaveArticleValidParamTheoryData))]
    public async Task ValidInputsInvalidUser(SaveArticleTheoryModel data)
    {
        // Arrange
        _ctxFixture.AddHttpClient(
            response: new SaveArticleResponse
            {
                Id = 0
            },
            endpoint: "api/articles/save-article");

        var cut = _ctxFixture.Context.RenderComponent<AddArticlePage>();

        // Act
        cut.Find("input#article-title").Change(data.Title);
        cut.Find("textarea#article-content").Change(data.Content);
        await cut.Find("button#submit").ClickAsync(null!);

        // Assert
        SnackbarService.ShownSnackbars.Count().ShouldBe(1);
        var snackbar = SnackbarService.ShownSnackbars.First();

        snackbar.Message.ShouldBe("Completed successfully");
        snackbar.Severity.ShouldBe(Severity.Success);
    }
}
