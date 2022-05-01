using Articles.Test.Helper.Fixture;

namespace Articles.Api.Test.Features.Login;

public class LoginServiceServiceBadTypeCollectionFixture : DbAbstractServiceCollectionFixture<ArticleReadOnlyContext>
{
    protected override IServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection()
            .SetupBasicesConfigurationForServices(
               interfaceType: typeof(ILoginService),
               implemantationType: typeof(LoginEndpoint))
            .BuildServiceProvider();
    }

    protected override ArticleReadOnlyContext GetDbContext() =>
        ServiceProvider.GetRequiredService<ArticleReadOnlyContext>();
}
