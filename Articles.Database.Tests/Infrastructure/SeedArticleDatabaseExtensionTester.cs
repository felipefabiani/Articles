using Articles.Database.Infrastructure;
using Articles.Test.Helper.Bases;
using Articles.Test.Helper.Fixture;

namespace Articles.Database.Tests.Infrastructure;

public class SeedArticleDatabaseExtensionTester :
    ContextDb<ServiceCollectionFixture>
{
    protected override void SeedDb() { }

    public SeedArticleDatabaseExtensionTester(ServiceCollectionFixture spFixture) :
        base(spFixture)

    {
    }

    [Fact]
    public async Task Seed()
    {
        await _contextWriteOnly.Seed();

        _contextWriteOnly.Users.Count().ShouldBe(4);
    }
    [Fact]
    public async Task SeedUsers()
    {
        await _contextWriteOnly.SeedUsers();

        _contextWriteOnly.Users.Count().ShouldBe(4);
    }
}