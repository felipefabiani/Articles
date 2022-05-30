namespace Articles.Client.Test.Specs.Features.Pages.Articles.AddNewArticle;

[Binding, Scope(Feature = "AddNewArticleMenu")]
public class AddNewArticleMenuStepDefinitions : IDisposable
{
    private SaveArticlePageObject _page = default!;

    [Given(@"A logged Author")]
    public async Task GivenALoggedAuthor()
    {
        _page = await SaveArticlePageObject.Create<SaveArticlePageObject>(
            token: SaveArticlePageObject.AuthorToken,
            gotoPage: false).ConfigureAwait(false);
    }

    [When(@"author attempts to navigate to Add Article")]
    public async Task WhenAuthorAttemptsToNavigateToAddArticle()
    {
        await _page.ClickArticleMenu().ConfigureAwait(false);
        await _page.ClickAddArticleMenu().ConfigureAwait(false);
    }

    [Then(@"open add article page")]
    public async Task ThenOpenAddArticlePage()
    {
        (await _page.IsPageLoad().ConfigureAwait(false)).ShouldBe(true);
    }

    [Given(@"A logged User")]
    public async Task GivenALoggedUser()
    {
        _page = await SaveArticlePageObject.Create<SaveArticlePageObject>(
            token: SaveArticlePageObject.NonAuthorToken,
            gotoPage: false).ConfigureAwait(false);
    }

    [When(@"user attempts to navigate to Add Article")]
    public async Task WhenUserAttemptsToNavigateToAddArticle()
    {
        (await _page.HasArticleMenu().ConfigureAwait(false)).ShouldBe(false);
        (await _page.HasAddArticleMenu().ConfigureAwait(false)).ShouldBe(false);
    }

    [Then(@"get a not authorized message")]
    public async Task ThenGetANotAuthorizedMessage()
    {
        (await _page.IsPageLoad().ConfigureAwait(false)).ShouldBe(false);
    }

    public async void Dispose()
    {
        await _page.DisposeAsync().ConfigureAwait(false);
    }
}

