namespace Articles.Database.Context;

public class ArticleReadOnlyContext : ArticleContext
{
    public ArticleReadOnlyContext(DbContextOptions<ArticleContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
}
