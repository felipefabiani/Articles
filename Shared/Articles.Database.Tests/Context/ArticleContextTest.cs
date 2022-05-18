using Articles.Test.Helper.Fixture;

namespace Articles.Database.Tests.Context;
public class ArticleContextTest
      : IClassFixture<ServiceCollectionFixture>
{
    private readonly ArticleContext _context;

    public ArticleContextTest(ServiceCollectionFixture spFixture)
    {
        _context = spFixture.ServiceProvider.GetRequiredService<ArticleContext>();
    }

    [Fact]
    public async Task Test()
    {
        _context.ChangeTracker.QueryTrackingBehavior.ShouldBe(QueryTrackingBehavior.TrackAll);
        Should.NotThrow(() => _context.SaveChanges());
        await Should.NotThrowAsync(async () => await _context.SaveChangesAsync(false));
    }
}
