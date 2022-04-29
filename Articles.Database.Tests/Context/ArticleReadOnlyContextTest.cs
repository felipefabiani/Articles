using Articles.Api.Test.Features.Login;
using Articles.Database.Context;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Articles.Database.Tests.Context;
public class ArticleReadOnlyContextTest
      : IClassFixture<ArticleReadOnlyContextServiceServiceCollectionFixture>
{
    private readonly ArticleReadOnlyContext _context;

    public ArticleReadOnlyContextTest(ArticleReadOnlyContextServiceServiceCollectionFixture spFixture)
    {
        _context =  spFixture.ServiceProvider.GetRequiredService<ArticleReadOnlyContext>();    
    }

    [Fact]
    public async Task Test()
    {
        _context.ChangeTracker.QueryTrackingBehavior.ShouldBe(QueryTrackingBehavior.NoTracking);

        Should.Throw<Exception>(() => _context.SaveChanges())
            .Message.ShouldBe("Do not save data from this context");

        (await Should.ThrowAsync<Exception>(async () => await _context.SaveChangesAsync(false)))
            .Message.ShouldBe("Do not save data from this context");
    }
}
