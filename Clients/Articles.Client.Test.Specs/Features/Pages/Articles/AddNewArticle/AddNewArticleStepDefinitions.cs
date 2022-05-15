namespace Articles.Client.Test.Specs.Features.Pages.Articles.AddNewArticle;

[Binding]
public class AddNewArticleStepDefinitions : IDisposable
{
    private SaveArticlePageObject _page;

    [Given(@"A logged Author")]
    public async Task GivenALoggedAuthor()
    {
        _page = await SaveArticlePageObject.Create<SaveArticlePageObject>(SaveArticlePageObject.AuthorToken);
    }

    [When(@"author attempts to add new article with invalid title and content")]
    public async Task WhenAuthorAttemptsToAddNewArticleWithInvalidTitleAndContent(Table table)
    {
        _page.SetData(table);
        await _page.SetTitle(_page.Data.Title);
        await _page.SetContent(_page.Data.Content);
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

    public async void Dispose()
    {
        await _page.DisposeAsync().ConfigureAwait(false);
    }
}

