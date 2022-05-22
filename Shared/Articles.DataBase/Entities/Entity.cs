using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Articles.Database.Entities;

public abstract class Entity : IEntity
{
    public int Id { get; set; }
}

public abstract class Entity<T> : Entity, IAsyncDisposable
    where T : class, IEntity
{
    public Entity() { }
    public Entity(IServiceProvider service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = service
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger<T>();

        _contextReadOnly = service.
            GetRequiredService<IDbContextFactory<ArticleReadOnlyContext>>()
            .CreateDbContext();

        _contextWriteOnly= service.
            GetRequiredService<IDbContextFactory<ArticleContext>>()
            .CreateDbContext();
    }

    protected readonly IServiceProvider _service = null!;
    protected readonly ILogger<T> _logger = null!;
    private readonly ArticleContext _contextWriteOnly;
    private readonly ArticleReadOnlyContext _contextReadOnly;

    protected DbSet<T> DbSetWriteOnly
        => _contextWriteOnly.Set<T>();
    protected DbSet<T> DbSetReadOnly
        => _contextReadOnly.Set<T>();
    protected async Task SaveAsync(CancellationToken cancellation)
        => await _contextWriteOnly.SaveChangesAsync(cancellation);

    public async ValueTask DisposeAsync()
    {
        if (_contextWriteOnly is not null)
        {
            await _contextWriteOnly.DisposeAsync();
        }

        if (_contextReadOnly is not null)
        {
            await _contextReadOnly.DisposeAsync();
        }
    }
}
