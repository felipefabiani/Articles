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


    protected readonly IServiceProvider? _service;
    protected readonly ILogger<T>? _logger;


    private DbContext GetDbContext<TDbContext>()
        where TDbContext : DbContext
    {
        return _dbContextWriteOnly ?? (
            _dbContextWriteOnly = _service.GetRequiredService<TDbContext>());
    }
    private DbSet<T> GetDbSet<TDbContext>()
        where TDbContext : DbContext
    {
        var c = GetDbContext<TDbContext>();
        return c.Set<T>();
    }

    private DbContext _dbContextWriteOnly = null!;
    private DbSet<T> _writeOnly = null!;
    private DbSet<T> _readOnly = null!;
    protected DbSet<T> DbSetWriteOnly =>
        _writeOnly ?? (_writeOnly = GetDbSet<ArticleContext>());

    protected DbSet<T> DbSetReadOnly =>
        _readOnly ?? (_readOnly = GetDbSet<ArticleReadOnlyContext>());

    protected async Task SaveAsync(CancellationToken cancellation) =>
        await GetDbContext<ArticleContext>().SaveChangesAsync(cancellation);

}
