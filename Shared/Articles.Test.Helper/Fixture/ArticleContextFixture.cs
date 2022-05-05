using Microsoft.Extensions.DependencyInjection;

namespace Articles.Test.Helper.Fixture;

public class ServiceCollectionFixture : AbstractServiceCollectionFixture
{
}

public abstract class AbstractServiceCollectionFixture : IDisposable
{
    public IServiceProvider ServiceProvider { get; private set; }
    public AbstractServiceCollectionFixture()
    {
        ServiceProvider = BuildServiceProvider();
    }
    protected virtual IServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection()
            .SetupBasicesConfigurationForServices()
            .BuildServiceProvider();
    }

    public void Dispose() => (ServiceProvider as ServiceProvider)?.Dispose();
}