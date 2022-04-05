using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
namespace Articles.Api.Test.Fixture;

public static class ServiceCollectionTestExtension
{
    public static IServiceCollection SetupBasicesConfigurationForServices<TDb>(
        this IServiceCollection services,
        Type interfaceType,
        Type implemantationType)
        where TDb : ArticleAbstractContext
    {
        if (!interfaceType.IsAssignableFrom(implemantationType))
        {
            throw new Exception("ImplemantationType must extend interfaceType");
        }

        return services
            .AddDbContext<TDb>()
            .ConfigureOptions()
            .AddNullLogger()
            .AddSingleton(interfaceType, implemantationType);

    }
    public static IServiceCollection AddDbContext<T>(this IServiceCollection services)
        where T : ArticleAbstractContext
    {
        return services.AddDbContext<T>(options => options
            .UseInMemoryDatabase(
                databaseName: Guid.NewGuid().ToString()),
                optionsLifetime: ServiceLifetime.Transient);
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
