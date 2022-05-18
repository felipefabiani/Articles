using Articles.Test.Helper.Fixture;

namespace Articles.Database.Tests.Context;
public class ArticleReadOnlyContextTest
      : IClassFixture<ServiceCollectionFixture>
{
    private readonly ArticleReadOnlyContext _context;

    public ArticleReadOnlyContextTest(ServiceCollectionFixture spFixture)
    {
        _context = spFixture.ServiceProvider.GetRequiredService<ArticleReadOnlyContext>();
    }

    [Fact]
    public async Task SaveAsync_should_throw_exception()
    {
        _context.ChangeTracker.QueryTrackingBehavior.ShouldBe(QueryTrackingBehavior.NoTracking);

        Should.Throw<Exception>(() => _context.SaveChanges())
            .Message.ShouldBe("Do not save data from this context");

        (await Should.ThrowAsync<Exception>(async () => await _context.SaveChangesAsync(false)))
            .Message.ShouldBe("Do not save data from this context");
    }
}
