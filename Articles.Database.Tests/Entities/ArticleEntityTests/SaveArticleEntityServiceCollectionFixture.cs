﻿using Articles.Test.Helper.Fixture;

namespace Articles.Database.Tests.Context;

public class SaveArticleEntityServiceCollectionFixture :
    DbAbstractServiceCollectionFixture<ArticleContext>
{
    protected override IServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection()
            .SetupBasicesConfigurationForServices()
            .BuildServiceProvider();
    }

    protected override ArticleContext GetDbContext() =>
        ServiceProvider.GetRequiredService<ArticleContext>();

    protected override void InitDb() { }
}