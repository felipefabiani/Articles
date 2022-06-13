using Articles.Api.Features.Author.Query;
using Articles.Database.View;
using Articles.Test.Helper.Bases;
using Articles.Test.Helper.Fixture;
using Articles.Test.Helper.TheoryData;
using Microsoft.AspNetCore.Http;

namespace Articles.Api.Test.Features.Author.Queries;

public class GetAuthorsEndpointTester :
    ContextDb<ServiceCollectionFixture>
{
    private readonly DefaultHttpContext _defaultHttpContext;

    public GetAuthorsEndpointTester(
        ServiceCollectionFixture fixture) :
        base(fixture)
    {
        _defaultHttpContext = _spFixture.ServiceProvider.GetRequiredService<DefaultHttpContext>();
    }
    protected override void SeedDb()
    {
        _contextWriteOnly.AddRange(
            new AuthorView { Id = 1, FirstName = "Full", LastName = "Access" },
            new AuthorView { Id = 2, FirstName = "Author", LastName = "Test" });
        _contextWriteOnly.SaveChanges();
    }

    [Fact]
    public async Task ToEntityAsync()
    {
        var sut = Factory.Create<GetAuthorsEndpoint>(_defaultHttpContext, default);

        await sut.HandleAsync(default!, default);

        sut.Response.Count.ShouldBe(2);

        foreach (var article in sut.Response)
        {
            article.Id.ShouldBeGreaterThan(0);
            article.FirstName.ShouldNotBeNullOrWhiteSpace();
            article.LastName.ShouldNotBeNullOrWhiteSpace();
            article.FullName.ShouldBe($"{article.FirstName} {article.LastName}");
        }
    }
}