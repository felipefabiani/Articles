using Articles.Database.Context;
using Articles.Database.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;

namespace Articles.Api.Test.Features.Login;

public class ArticleReadOnlyContextServiceServiceCollectionFixture :
    DbAbstractServiceCollectionFixture<ArticleReadOnlyContext>
{
    protected override ServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection()
            .AddDbContext<ArticleReadOnlyContext>()
            .BuildServiceProvider();
    }

    protected override ArticleReadOnlyContext GetDbContext() =>
        ServiceProvider.GetRequiredService<ArticleReadOnlyContext>();

    protected override void InitDb() {}
}

public class ArticleContextServiceCollectionFixture :
    DbAbstractServiceCollectionFixture<ArticleContext>
{
    protected override ServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection()
            .AddDbContext<ArticleContext>()
            .BuildServiceProvider();
    }

    protected override ArticleContext GetDbContext() =>
        ServiceProvider.GetRequiredService<ArticleContext>();

    protected override void InitDb() { }
}