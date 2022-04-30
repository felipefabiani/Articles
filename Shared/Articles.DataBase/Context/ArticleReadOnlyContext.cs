namespace Articles.Database.Context;

public class ArticleReadOnlyContext : ArticleAbstractContext
{
    public ArticleReadOnlyContext(DbContextOptions<ArticleReadOnlyContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
    
    public override int SaveChanges()
    {
        throw new Exception("Do not save data from this context");
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        throw new Exception("Do not save data from this context");
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        throw new Exception("Do not save data from this context");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new Exception("Do not save data from this context");
    }
}