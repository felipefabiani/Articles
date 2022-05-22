using Articles.Test.Helper.Extensions;
namespace Articles.Client.Test.Specs.Features.Pages.Articles.AddNewArticle;

[Binding]
public class AddNewArticleStepDefinitions : IDisposable
{
    private SaveArticlePageObject _page = default!;

    [Given(@"A logged Author")]
    public async Task GivenALoggedAuthor()
    {
        _page = await SaveArticlePageObject.Create<SaveArticlePageObject>(SaveArticlePageObject.AuthorToken);
    }

    [When(@"author attempts to add new article with invalid title and content")]
    public async Task WhenAuthorAttemptsToAddNewArticleWithInvalidTitleAndContent(Table table)
    {
        await AddArticle(table);
        await _page.ClickAddButton();
    }

    [Then(@"article is not created")]
    public void ThenArticleIsNotCreated()
    {
        _page.Page.Url.ShouldBe(_page.GetUrl());
    }

    [Then(@"get messages for the inputs fields")]
    public async Task ThenGetMessagesForTheInputsFields()
    {
        (await _page.Page
            .Locator($"text={_page.Data.TitleMessage}")
            .CountAsync()
        ).ShouldBe(1);
        (await _page.Page
            .Locator($"text={_page.Data.ContentMessage}")
            .CountAsync()
        ).ShouldBe(1);
    }

    [When(@"author attempts to add new article with valid title and content")]
    public async Task WhenAuthorAttemptsToAddNewArticleWithValidTitleAndContent(Table table)
    {
        _page.SetData(table);
        await _page.SetTitle($"{_page.Data.Title} {10.RandomString()}");
        await _page.SetContent($"{_page.Data.Content} {100.RandomString()}");
    }

    [Then(@"article is created")]
    public async Task ThenArticleIsCreated()
    {
        await _page.Page.RunAndWaitForResponseAsync(async () =>
        {
            await _page.ClickAddButton();
        }, response =>
        {
            response.Url.ShouldContain("api/articles/save-article");
            response.Status.ShouldBe(200);
            return true;
        });
    }

    [Then(@"get message seccess message")]
    public async Task ThenGetMessageSeccessMessage()
    {
        (await _page.HasSuccessSnack("Completed successfully")).ShouldBe(true);
    }

    private async Task AddArticle(Table table)
    {
        _page.SetData(table);
        await _page.SetTitle(_page.Data.Title);
        await _page.SetContent(_page.Data.Content);
    }

    [Given(@"A logged User")]
    public async Task GivenALoggedUser()
    {
        _page = await SaveArticlePageObject.Create<SaveArticlePageObject>(SaveArticlePageObject.NonAuthorToken);
    }

    [When(@"user attempts to add go to the add article page")]
    public async Task WhenUserAttemptsToAddGoToTheAddArticlePage()
    {
        await Task.CompletedTask;
    }

    [Then(@"not authorized message")]
    public async void ThenNotAuthorizedMessage()
    {
        (await _page.HasNotAutorizedAccess()).ShouldBe(true);
    }

    public async void Dispose()
    {
        await _page.DisposeAsync().ConfigureAwait(false);
    }
}

