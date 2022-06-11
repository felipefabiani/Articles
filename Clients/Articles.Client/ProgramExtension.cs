using Articles.Client.Authentication;
using Articles.Helper.Auth;
using Blazored.LocalStorage;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using System.Reflection;

namespace Articles.Client;
public static class ProgramExtension
{
    public static void SetupApplication(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddOptions();
        builder.Services.AddArticlesAuthorization();
        builder.Services.AddScoped<IAuthStateProvider, AuthStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(p => (AuthStateProvider)p.GetRequiredService<IAuthStateProvider>());
        builder.Services.AddScoped<HotKeysHandle>();

        builder.Services.AddBlazoredLocalStorage();
        builder.AddMudServices();

        builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        builder.Services.AddScoped<AuthenticationHttpMessageHandler>();
        builder.Services
            .AddHttpClient("Article.Api", client =>
            {
                var ep = builder.Configuration["Endpoints:ArticleApi"] ?? string.Empty;
                client.BaseAddress = new Uri(ep);
            })
            .AddHttpMessageHandler<AuthenticationHttpMessageHandler>();
        builder.AddFluentValidators("Articles.Models");
    }
    public static void AddFluentValidators(this WebAssemblyHostBuilder builder, string assemblyName)
    {
        AddFluentValidators(builder, new List<string> { assemblyName });
    }
    public static void AddFluentValidators(this WebAssemblyHostBuilder builder, List<string> assemblyNames)
    {
        var validators = assemblyNames.GetAssemblies();

        foreach (var validator in validators)
        {
            var arg = validator.BaseType!.GetGenericArguments().First();
            var baseType = typeof(AbstractValidator<>).MakeGenericType(arg);
            builder.Services.AddSingleton(baseType, validator);
        }
    }

    public static List<Type> GetAssemblies(this List<string> assemblyNames)
    {
        var refs = Assembly
            .GetEntryAssembly()!
            .GetReferencedAssemblies()
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

    public static void AddMudServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddMudServices(MudConfig);
    }

    public static void MudConfig(MudServicesConfiguration config)
    {
        config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopEnd;
        config.SnackbarConfiguration.PreventDuplicates = false;
        config.SnackbarConfiguration.NewestOnTop = false;
        config.SnackbarConfiguration.ShowCloseIcon = true;
        config.SnackbarConfiguration.VisibleStateDuration = 5000;
        config.SnackbarConfiguration.HideTransitionDuration = 500;
        config.SnackbarConfiguration.ShowTransitionDuration = 500;
        config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
    }
}
