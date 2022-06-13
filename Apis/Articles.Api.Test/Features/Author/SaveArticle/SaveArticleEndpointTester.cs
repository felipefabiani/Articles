using Articles.Api.Features.Author.SaveArticle;
using Articles.Models.Feature.Articles.SaveArticle;
using Articles.Test.Helper.Bases;
using Articles.Test.Helper.Extensions;
using Articles.Test.Helper.Fixture;
using AutoFixture;
using Microsoft.AspNetCore.Http;

namespace Articles.Api.Test.Features.Author.SaveArticle;

public class SaveArticleEndpointTester :
    ContextDb<ServiceCollectionFixture>
{
    private readonly DefaultHttpContext _defaultHttpContext;

    public SaveArticleEndpointTester(
        ServiceCollectionFixture fixture) :
        base(fixture)
    {
        _defaultHttpContext = fixture.ServiceProvider.GetRequiredService<DefaultHttpContext>();
    }

    [Fact]
    public async Task ToEntityAsync()
    {
        var request = _fixture
            .Build<SaveArticleRequest>()
            .With(x => x.Id, 0)
            .With(x => x.AuthorId, 1)
            .WithStringLength(x => x.Title, 20)
            .WithStringLength(x => x.Content, 20)
            .Create();

        var sut = Factory.Create<GetAuthorsEndpoint>(_defaultHttpContext, default);

        await sut.HandleAsync(request, default);

        sut.Response.Id.ShouldBe(1);
    }
}