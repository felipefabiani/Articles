using Articles.Api.Features.Login;
using Articles.Database.Context;
using Articles.Database.Entities;
using Articles.Models.Feature.Articles.SaveArticle;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;

namespace Articles.Test.Helper.Fixture;

public static class ServiceCollectionTestExtension
{
    public static IServiceCollection SetupBasicesConfigurationForServices(
        this IServiceCollection services)
    {
        var test = new SaveArticleValidator(); // Load Article.Models to add Validators
        return services
            .AddDbContext()
            .ConfigureOptions()
            .AddNullLogger()
            // .AddFluentValidators("Articles.Models")
            .AddScoped<Testdb>()
            .AddSingleton((sp) => new DefaultHttpContext
            {
                RequestServices = sp
            })
            .AddTransient((sp) => new ArticleEntity(sp))
            .AddSingleton<ILoginService, LoginService>()
            .AddSingleton(new NexIdService());
    }

    private static readonly object obj = new object();
    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        return services
             .AddDbContextFactory<ArticleReadOnlyContext>((sp, options) =>
             {
                 var testdb = sp.GetRequiredService<Testdb>().Get();
                 options
                     .EnableSensitiveDataLogging()
                     .UseInMemoryDatabase(
                         databaseName: testdb.DatabaseName,
                         databaseRoot: testdb.DatabaseRoot,
                         options =>
                         {
                             options.EnableNullChecks();
                         });
             }, ServiceLifetime.Transient)

            .AddDbContextFactory<ArticleContext>((sp, options) =>
            {
                var testdb = sp.GetRequiredService<Testdb>().Get();
                options
                    .EnableSensitiveDataLogging()
                    .UseInMemoryDatabase(
                        databaseName: testdb.DatabaseName,
                        databaseRoot: testdb.DatabaseRoot,
                        options =>
                        {
                            options.EnableNullChecks();
                        });
            }, ServiceLifetime.Transient);
    }

    public class Testdb
    {
        public string DatabaseName { get; set; } = $"ArticleContext-{Guid.NewGuid()}";
        public InMemoryDatabaseRoot DatabaseRoot { get; set; } = new InMemoryDatabaseRoot();

        private int _id = 0;

        public void Reset(int id=0)
        {
            _id = id;
            _testdb = new Testdb
            {
                DatabaseName = $"ArticleContext-{_id}-{Guid.NewGuid()}",
                DatabaseRoot = new InMemoryDatabaseRoot()
            };
        }

        
        private Testdb _testdb = default!;
        public Testdb Get()
        {
            if (_testdb is null)
            {
                Reset(0);
            }

            return _testdb!;
        }
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
        return services
            .AddSingleton(typeof(ILogger<>), typeof(NullLogger<>))
            .AddSingleton<ILoggerFactory, NullLoggerFactory>();
    }

    public static IServiceCollection AddFluentValidators(this IServiceCollection services, string assemblyName)
    {
        return AddFluentValidators(services, new List<string> { assemblyName });
    }

    public static IServiceCollection AddFluentValidators(this IServiceCollection services, List<string> assemblyNames)
    {
        var validators = assemblyNames.GetAssemblies();

        foreach (var validator in validators)
        {
            // var arg = validator.BaseType!.GetGenericArguments().First();
            // var baseType = typeof(AbstractValidator<>).MakeGenericType(arg);
            services.AddSingleton(validator);
        }

        return services;
    }

    public static List<Type> GetAssemblies(this List<string> assemblyNames)
    {
        var refs = Assembly.GetCallingAssembly().GetReferencedAssemblies()
            .Where(assembly => assemblyNames.Any(name => assembly.FullName.Contains(name)))
            .Select(assembly => Assembly.Load(assembly))
            .ToList();

        return GetValidators(refs);
    }

    public static List<Type> GetValidators(this List<Assembly> assemblyNames)
    {
        return assemblyNames
            .SelectMany(assembly => assembly
                .GetTypes()
                .Where(t =>
                    !t.IsAbstract &&
                    t.BaseType is not null &&
                    t.BaseType.IsGenericType &&
                    t.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>)))
            .ToList() ??
            new List<Type>();
    }
}
