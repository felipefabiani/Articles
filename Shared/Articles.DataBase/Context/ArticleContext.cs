namespace Articles.Database.Context;
public class ArticleContext : ArticleAbstractContext
{
    public ArticleContext(DbContextOptions<ArticleContext> options)
        : base(options)
    {
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateTrackableEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        UpdateTrackableEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void UpdateTrackableEntities()
    {
        var selectedEntityList = ChangeTracker.Entries()
            .Where(x => x.Entity is ITrackableEntity)
            .ToList();

        foreach (var entity in selectedEntityList.Where(x => x.State == EntityState.Added).ToList())
        {
            var dtNow = DateTimeOffset.Now;
            ((ITrackableEntity)entity.Entity).CreatedOn = dtNow;
            ((ITrackableEntity)entity.Entity).LastModifyedOn = dtNow;
        }

        foreach (var entity in selectedEntityList.Where(x => x.State == EntityState.Modified).ToList())
        {
            ((ITrackableEntity)entity.Entity).LastModifyedOn = DateTimeOffset.Now;
        }
    }
}