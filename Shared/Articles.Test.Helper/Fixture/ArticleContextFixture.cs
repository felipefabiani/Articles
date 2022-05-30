using Microsoft.Extensions.DependencyInjection;

namespace Articles.Test.Helper.Fixture;

public class ServiceCollectionFixture : AbstractServiceCollectionFixture
{
}

public abstract class AbstractServiceCollectionFixture : 
    // IDisposable, 
    IAsyncDisposable
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

    // public void Dispose() => (ServiceProvider as ServiceProvider)?.Dispose();

    public async ValueTask DisposeAsync()
    {
        var sp = (ServiceProvider as ServiceProvider);
        if (sp is not null)
        {
            await sp.DisposeAsync();
            sp = null;
        }

        ServiceProvider = null!;
    }
}