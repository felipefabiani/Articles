using Articles.Database.View;
using Articles.Test.Helper.Bases;
using Articles.Test.Helper.Fixture;
using AutoFixture;

namespace Articles.Database.Tests.View.AuthorViewTests;
public class GetAuthorViewTester :
    ContextDb<ServiceCollectionFixture, AuthorView>
{
    public GetAuthorViewTester(
        ServiceCollectionFixture spFixture) :
        base(spFixture)
    {
    }

    protected override void SeedDb()
    {
        _contextWriteOnly.AddRange(
            new AuthorView { Id = 1, FirstName = "Full", LastName = "Access" },
            new AuthorView { Id = 2, FirstName = "Author", LastName = "Test" });
        _contextWriteOnly.SaveChanges();
    }

    [Fact]

    public async Task Get_Authors()
    {
        var sut = _entityBuilder
           .OmitAutoProperties()
           .Create();

        var ret = await sut.GetAuthors(default).ConfigureAwait(false);
        ret.Count.ShouldBe(2);
    }
}