using System.Reflection;

namespace Articles.Database.Context;

public abstract class ArticleAbstractContext : DbContext
{
    protected ArticleAbstractContext(DbContextOptions contextOptions)
        : base(contextOptions)
    {
    }

    public virtual DbSet<ArticleEntity> Articles { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Role> Roles { get; set; } = null!;
    public virtual DbSet<Claim> Claims { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
public class ArticleContext : ArticleAbstractContext
{
    public ArticleContext(DbContextOptions<ArticleContext> options)
        : base(options)
    {
    }
}

public class ArticleReadOnlyContext : ArticleAbstractContext
{
    public ArticleReadOnlyContext(DbContextOptions<ArticleReadOnlyContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess = true)
    {
        throw new Exception("Do not save data from this context");
    }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default)
    {
        throw new Exception("Do not save data from this context");
    }
}