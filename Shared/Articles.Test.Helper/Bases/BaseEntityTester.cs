using Articles.Database.Context;
using Articles.Database.Entities;
using Articles.Test.Helper.Fixture;
using AutoFixture;
using AutoFixture.Dsl;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Articles.Test.Helper.Bases;
public abstract class BaseEntityTester<TClassFixture, TEntity> :
    IClassFixture<TClassFixture>,
    IDisposable
    where TClassFixture : AbstractServiceCollectionFixture
    where TEntity : Entity<TEntity>, new()
{
    protected readonly IPostprocessComposer<TEntity> _entityBuilder;
    protected readonly IFixture _fixture;
    protected readonly TClassFixture _spFixture;
    protected readonly ArticleContext _context;

    protected BaseEntityTester(TClassFixture spFixture, TEntity entityBuilder)
    {
        _spFixture = spFixture;
        _fixture = new AutoFixture.Fixture();
        _context = _spFixture.ServiceProvider.GetRequiredService<ArticleContext>();

        PreEntityBuilder();

        _entityBuilder = _fixture
            .Build<TEntity>()
            .FromFactory(() => entityBuilder);
    }

    protected virtual void PreEntityBuilder() { }

    protected virtual void ResetDb()
    {
        var entities = _context
            .Set<TEntity>()
            .ToList();

        if (entities.Count == 0)
        {
            return;
        }

        _context.RemoveRange(entities);
        _context.SaveChanges();
    }
    public void Dispose()
    {
        _context.ChangeTracker.Clear();
        ResetDb();
    }
}
