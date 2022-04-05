namespace Articles.Api.Test.Features.Login;

public class LoginServiceServiceCollectionFixture : DbAbstractServiceCollectionFixture<ArticleReadOnlyContext>
{
    protected override ServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection()
            .SetupBasicesConfigurationForServices<ArticleReadOnlyContext>(
               interfaceType: typeof(ILoginService),
               implemantationType: typeof(LoginService))
            .BuildServiceProvider();
    }

    protected override ArticleReadOnlyContext GetDbContext() =>
        ServiceProvider.GetRequiredService<ArticleReadOnlyContext>();
}
