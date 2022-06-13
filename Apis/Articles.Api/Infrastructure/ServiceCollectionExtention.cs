using Articles.Database.Infrastructure;
using Articles.Helper;
using Articles.Helper.Auth;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Scrutor;
using Serilog;
using System.Reflection;
using System.Text.Json;

using static Articles.Helper.ArticlesConstants.Security;
using static System.Net.Mime.MediaTypeNames;

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
        AddCors();
        AddAuthentication();

        builder.Services.AddTransient((sp) => new ArticleEntity(sp));

        _ = builder.Services.AddArticlesAuthorization();
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

            builder.Services.AddDbContextFactory<ArticleContext>(options =>
            {
                options
#if DEBUG
                    .EnableSensitiveDataLogging()
#endif
                    .UseSqlServer(connectionString);
            });

            builder.Services.AddDbContextFactory<ArticleReadOnlyContext>(options =>
            {
                options
#if DEBUG
                    .EnableSensitiveDataLogging()
#endif
                    .UseSqlServer(connectionString);
            });

            builder.Services.AddDbContext<ArticleReadOnlyContext>(options =>
            {
                options
#if DEBUG
                    .EnableSensitiveDataLogging()
#endif
                    .UseSqlServer(connectionString);
            });
            builder.Services.AddDbContext<ArticleContext>(options =>
            {
                options
#if DEBUG
                    .EnableSensitiveDataLogging()
#endif
                    .UseSqlServer(connectionString, x =>
                    {
                        x.MigrationsAssembly(typeof(ArticleContext).Assembly.FullName);
                        x.EnableRetryOnFailure(2);
                    });

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
        void AddCors()
        {
            var allowedHosts = builder.Configuration
                .GetSection(ArticlesConstants.Cors.AllowedHostsSection)
                .Get<string[]>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    name: ArticlesConstants.Cors.ArticlesClient,
                    builder =>
                    {
                        builder
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .WithOrigins(allowedHosts);
                    });
            });
        }
        void AddAuthentication()
        {
            var jwt = builder.Configuration["ArticleOptions:JwtSigningKey"];
            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = "Bearer";
                o.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(delegate (JwtBearerOptions o)
            {
                var issuerSigningCertificate = new SigningIssuerCertificate();
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwt)),
                    IssuerSigningKey = issuerSigningCertificate.GetIssuerSigningKey(),

                    ValidateAudience = true,
                    ValidAudience = Audience,
                    ValidateIssuer = true,
                    ValidIssuer = Issuer,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }

    public static async Task<WebApplication> SetApplication(this WebApplication app)
    {
        await SetEnvironment();

        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = Text.Plain;
                await context.Response.WriteAsync("An exception was thrown.");

                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var looger = app.Services.GetRequiredService<Serilog.ILogger>();

                looger.Error(exceptionHandlerPathFeature.Error.Message, exceptionHandlerPathFeature.Error);
            });
        });
        app.UseCors(ArticlesConstants.Cors.ArticlesClient);
        app.UseAuthentication();
        app.UseAuthorization();

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


        //app.Run(async context =>
        //{
        //    context.Response.Redirect("swagger");
        //    await Task.CompletedTask;
        //});
        return app;

        async Task<WebApplication> SetEnvironment()
        {
            if (app.Environment.IsDevelopment() ||
                app.Environment.IsEnvironment(ArticlesConstants.Environment.Uat))
            {
                //app.UseOpenApi();
                //app.UseSwaggerUi3(c => c.ConfigureDefaults());


                var factory = app.Services.GetRequiredService<IDbContextFactory<ArticleContext>>();
                using var context = factory.CreateDbContext();
                await context.EnsureCreateAndSeedAsync();
                await context.Seed();
            }
            else if (app.Environment.IsEnvironment(ArticlesConstants.Environment.UatAutomatedTest))
            {
#if DEBUG
                app.UseOpenApi();
                app.UseSwaggerUi3(c => c.ConfigureDefaults());
#endif
                try
                {
                    var factory = app.Services.GetRequiredService<IDbContextFactory<ArticleContext>>();
                    using var context = factory.CreateDbContext();
                    await context.EnsureDropCreateAndSeedAsync();
                    await context.Seed();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            return app;
        }
    }
}
