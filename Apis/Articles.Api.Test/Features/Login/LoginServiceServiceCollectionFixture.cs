using Articles.Test.Helper.Fixture;

namespace Articles.Api.Test.Features.Login;

public class LoginServiceServiceCollectionFixture : DbAbstractServiceCollectionFixture<ArticleReadOnlyContext>
{
    protected override IServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection()
            .SetupBasicesConfigurationForServices(
               interfaceType: typeof(ILoginService),
               implemantationType: typeof(LoginService))
            .BuildServiceProvider();
    }

    protected override ArticleReadOnlyContext GetDbContext() =>
        ServiceProvider.GetRequiredService<ArticleReadOnlyContext>();
}
