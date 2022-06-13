using Articles.Api.Features.Admin.QueryArticles;
using Articles.Test.Helper.Bases;
using Articles.Test.Helper.Fixture;
using Articles.Test.Helper.TheoryData;
using Microsoft.AspNetCore.Http;

namespace Articles.Api.Test.Features.Author.Queries;

public class GetPendingApprovalArticlesEndpointTester :
    ContextDb<ServiceCollectionFixture>
{
    private readonly DefaultHttpContext _defaultHttpContext;

    public GetPendingApprovalArticlesEndpointTester(
        ServiceCollectionFixture fixture) :
        base(fixture)
    {
        _defaultHttpContext = _spFixture.ServiceProvider.GetRequiredService<DefaultHttpContext>();
    }

    [Theory]
    [ClassData(typeof(PendingApprovalArticlesValidQueryParamTheoryData))]
    public async Task ToEntityAsync(
        PendingApprovalArticlesModelData data)
    {
        ArticleSeed();
        var sut = Factory.Create<GetPendingApprovalArticlesEndpoint>(_defaultHttpContext, default);

        await sut.HandleAsync(data.Data, default);

        sut.Response.Count.ShouldBe(data.Total);

        foreach (var article in sut.Response)
        {
            article.Id.ShouldBeGreaterThan(0);
            article.Title.ShouldNotBeNullOrWhiteSpace();
            article.Content.ShouldNotBeNullOrWhiteSpace();
            article.AuthorName.ShouldNotBeNullOrWhiteSpace();
            article.CreatedOn.ShouldNotBe(DateTimeOffset.UtcNow);
            article.CreatedOn.ShouldNotBe(default);
        }
    }
}