using Articles.Database.Context;
using Articles.Test.Helper.Fixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Articles.Test.Helper.Fixture;

public static class ServiceCollectionTestExtension
{
    public static IServiceCollection SetupBasicesConfigurationForServices(
        this IServiceCollection services,
        Type interfaceType,
        Type implemantationType)
    {
        if (!interfaceType.IsAssignableFrom(implemantationType))
        {
            throw new Exception("ImplemantationType must extend interfaceType");
        }

        return services
            .AddDbContext<ArticleContext>()
            .AddDbContext<ArticleReadOnlyContext>()
            .ConfigureOptions()
            .AddNullLogger()
            .AddSingleton(interfaceType, implemantationType);

    }

    private static readonly InMemoryDatabaseRoot _inMemoryDatabaseRoot = new InMemoryDatabaseRoot();
    public static IServiceCollection AddDbContext<T>(this IServiceCollection services)
        where T : ArticleAbstractContext
    {
        return services.AddDbContext<T>(options =>
        {
            options.UseInMemoryDatabase(
                databaseName: "ArticleContext",
                databaseRoot: _inMemoryDatabaseRoot

                );
        }, optionsLifetime: ServiceLifetime.Transient);

    }
    public static IServiceCollection ConfigureOptions(this IServiceCollection services)
    {
        return services.Configure<ArticleOptions>(configureOptions =>
        {
            configureOptions.JwtSigningKey = Guid.NewGuid().ToString();
            configureOptions.SaltId = Guid.NewGuid().ToString();
        });
    }
    public static IServiceCollection AddNullLogger(this IServiceCollection services)
    {
        return services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
    }
}
