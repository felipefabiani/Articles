namespace Articles.Client.Test.Specs.Features.Pages.Articles.AddNewArticle;

[Binding, Scope(Feature = "AddNewArticleMenu")]
public class AddNewArticleMenuStepDefinitions : IDisposable
{
    private SaveArticlePageObject _page = default!;

    [Given(@"A logged Author")]
    public async Task GivenALoggedAuthor()
    {
        _page = await SaveArticlePageObject.Create<SaveArticlePageObject>(SaveArticlePageObject.AuthorToken, false);
    }

    [When(@"author attempts to navigate to Add Article")]
    public async Task WhenAuthorAttemptsToNavigateToAddArticle()
    {
        await _page.ClickArticleMenu();
        await _page.ClickAddArticleMenu();
    }

    [Then(@"open add article page")]
    public async Task ThenOpenAddArticlePage()
    {
        (await _page.IsPageLoad()).ShouldBe(true);
    }

    [Given(@"A logged User")]
    public async Task GivenALoggedUser()
    {
        _page = await SaveArticlePageObject.Create<SaveArticlePageObject>(SaveArticlePageObject.NonAuthorToken, false);
    }

    [When(@"user attempts to navigate to Add Article")]
    public async Task WhenUserAttemptsToNavigateToAddArticle()
    {
        await _page.ClickArticleMenu();
        await _page.ClickAddArticleMenu();
    }

    [Then(@"get a not authorized message")]
    public async Task ThenGetANotAuthorizedMessage()
    {
        (await _page.IsPageLoad()).ShouldBe(false);
    }

    public async void Dispose()
    {
        await _page.DisposeAsync().ConfigureAwait(false);
    }
}

