using Articles.Database.Infrastructure;
namespace Articles.Database.Tests.Infrastructure;

public class SeedArticleDatabaseExtensionTester :
    IClassFixture<ArticleContextServiceCollectionFixture>
{
    private readonly ArticleContext _context;

    public SeedArticleDatabaseExtensionTester(ArticleContextServiceCollectionFixture spFixture)
    {
        _context = spFixture.ServiceProvider.GetRequiredService<ArticleContext>();
    }

    [Fact]
    public async Task Seed()
    {
        await _context.Seed();

        _context.Users.Count().ShouldBe(4);
    }
    [Fact]
    public async Task SeedUsers()
    {
        await _context.SeedUsers();

        _context.Users.Count().ShouldBe(4);
    }
}