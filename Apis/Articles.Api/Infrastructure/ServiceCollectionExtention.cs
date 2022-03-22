using Articles.Database.Infrastructure;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scrutor;
using Serilog;
using System.Reflection;
using System.Text.Json;

namespace Articles.Api.Infrastructure;

public static class ServiceCollectionExtention
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        AddSerilog();
        AddConfiguration();
        AddSwaggerDoc();
        AddDbContext();
        AddLifetimeServices();
        AddDecorators();

        _ = builder.Services.AddFastEndpoints();
        _ = builder.Services.AddResponseCaching();

        return builder;

        void AddSerilog()
        {
            builder.Host.UseSerilog((context, config) =>
            {
                config
                    .WriteTo.Console()
                    .WriteTo.File(ArticlesConstants.LogFilePath, rollingInterval: RollingInterval.Day);
            });
        }
        void AddConfiguration()
        {
            builder.Services.Configure<ArticleOptions>(
                builder.Configuration.GetSection(ArticlesConstants.ArticlesOptions));
        }
        void AddSwaggerDoc()
        {
            builder.Services.AddSwaggerDoc(s =>
            {
                s.DocumentName = "Release 1.0";
                s.Title = "Web API";
                s.Version = "v1.0";
            });
        }
        void AddDbContext()
        {
            var connectionString = builder.Configuration.GetConnectionString(ArticlesConstants.ConnectionString);

            builder.Services.AddDbContext<ArticleContext>(options =>
            {
                options
                    .UseSqlServer(connectionString, x =>
                    {
                        x.MigrationsAssembly(typeof(ArticleContext).Assembly.FullName);
                    });
    #if DEBUG
                options.EnableSensitiveDataLogging();
    #endif
            });
            builder.Services.AddDbContext<ArticleReadOnlyContext>(options =>
            {
                options.UseSqlServer(connectionString);

    #if DEBUG
                options.EnableSensitiveDataLogging();
    #endif
            });
        }
        void AddLifetimeServices()
        {
            builder.Services.Scan(scan => scan
                .FromAssemblyOf<ITransientService>()
                    .AddClasses(classes => classes.AssignableTo<ITransientService>())
                        .AsImplementedInterfaces()
                        .WithTransientLifetime()

                    .AddClasses(classes => classes.AssignableTo<IScopedService>())
                        .AsImplementedInterfaces()
                        .WithScopedLifetime()

                    .AddClasses(classes => classes.AssignableTo<ISingletonService>())
                        .AsImplementedInterfaces()
                        .WithSingletonLifetime()

                    .AddClasses(classes => classes.WithAttribute<DecoratorServiceAttribute>())
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsSelf()
            );
        }
        void AddDecorators()
        {
            var groupTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttribute<DecoratorServiceAttribute>()?.Order >= 0)
                .Where(t => t.GetCustomAttribute<DecoratorServiceAttribute>()?.Skip == false)
                .GroupBy(
                    key => key.GetCustomAttribute<DecoratorServiceAttribute>()?.Type,
                    type => type,
                    (key, types) => new
                    {
                        Key = key,
                        Types = types
                            .OrderBy(t => t.GetCustomAttribute<DecoratorServiceAttribute>()?.Order)
                            .ToList()
                    })
                .ToList();

            foreach (var types in groupTypes)
            {
                for (var i = 0; i < types.Types.Count - 1; i++)
                {
                    builder.Services.Decorate(types.Key!, types.Types[i]);
                }
                builder.Services.Decorate(
                    types.Key!,
                    (inner, provider) =>
                    {
                        var type = types.Types.Last();
                        var decorator = DecoratorFactory.GetDecorator(type, inner, provider);

                        if (decorator is not null)
                        {
                            return decorator;
                        }

                        var args = type.GetConstructors()[0]
                            .GetParameters()
                            .Select(x =>
                            {
                                if (x.ParameterType == types.Key)
                                {
                                    return inner;
                                }

                                return provider.GetRequiredService(x.ParameterType);
                            })
                            .ToArray();

                        return Activator.CreateInstance(type, args)!;
                    });
            }
        }
    }

    public static async Task<WebApplication> SetApplication(this WebApplication app)
    {
        await SetEnvironment();

        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();

        app.UseResponseCaching();
        app.UseFastEndpoints(s =>
        {
            s.RoutingOptions = o =>
            {
                o.Prefix = "api";
            };
            s.SerializerOptions = o =>
            {
                o.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            };
        });

        return app;

        async Task<WebApplication> SetEnvironment()
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi3(c => c.ConfigureDefaults());

                // var context = app.Services.GetRequiredService<ArticleContext>();
                // await context.EnsureCreateAndSeedAsync();
                // await context.Seed();
            }
            else if (app.Environment.IsEnvironment(ArticlesConstants.Environment.UatAutomatedTest))
            {
#if DEBUG
                app.UseOpenApi();
                app.UseSwaggerUi3(c => c.ConfigureDefaults());
#endif
                var context = app.Services.GetRequiredService<ArticleContext>();
                await context.EnsureDropCreateAndSeedAsync();
                await context.Seed();
            }

            return app;
        }
    }    
}
