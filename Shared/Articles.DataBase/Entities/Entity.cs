using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Articles.Database.Entities;

public abstract class Entity : IEntity
{
    public int Id { get; set; }
}

public abstract class Entity<T> : Entity
    where T : class, IEntity
{
    public Entity() { }
    public Entity(IServiceProvider service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = service
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger<T>();
    }

    protected readonly IServiceProvider _service = null!;
    protected readonly ILogger<T> _logger = null!;

    private DbContext GetDbContext<TDbContext>() where TDbContext : DbContext
        => _service.GetRequiredService<TDbContext>();
    private DbSet<T> GetDbSet<TDbContext>() where TDbContext : DbContext
        => GetDbContext<TDbContext>().Set<T>();
    protected DbSet<T> DbSetWriteOnly
        => GetDbSet<ArticleContext>();
    protected DbSet<T> DbSetReadOnly
        => GetDbSet<ArticleReadOnlyContext>();
    protected async Task SaveAsync(CancellationToken cancellation)
        => await GetDbContext<ArticleContext>().SaveChangesAsync(cancellation);
}
