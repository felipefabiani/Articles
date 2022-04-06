using Articles.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
using Serilog.Debugging;
using MudBlazor.Services;
using FluentValidation;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

SelfLog.Enable(m => Console.Error.WriteLine(m));
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.BrowserConsole()
    .CreateLogger();


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
Log.Debug("Adding services");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient<ArticleApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7183");
    //client.DefaultRequestHeaders.Add("accept", "application/json");
    //client.DefaultRequestHeaders.Add("Content-Type", "application/json");

});

List<Assembly> listOfAssemblies = new List<Assembly>();
var mainAsm = Assembly.GetEntryAssembly();
listOfAssemblies.Add(mainAsm);

foreach (var refAsmName in mainAsm.GetReferencedAssemblies())
{
    listOfAssemblies.Add(Assembly.Load(refAsmName));
}


var ass = listOfAssemblies
    .Where(x => x.FullName.Contains("Articles"));

var validators = listOfAssemblies
    .SelectMany(x => x.GetTypes())
    .ToList();

var validators1 = validators
    // .Where(x => x.Name == "UserLoginRequestValidator")
    // .Where(t => t.IsAssignableTo(typeof(AbstractValidator<UserLoginRequest>)))
    .Where(x => x.FullName.Contains("Articles"))
    .Where(t =>
        t.BaseType is not null &&
        !t.IsAbstract &&
        t.BaseType.IsGenericType &&
        t.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>))
    .ToList();

foreach (var item in validators1)
{
    var arg = item.BaseType!.GetGenericArguments().First();
    var genericInterface = typeof(AbstractValidator<>).MakeGenericType(arg);
    builder.Services.AddSingleton(genericInterface, item);
}



Log.Debug("Setting Oidc");
builder.Services.AddOidcAuthentication(options =>
{
    // Configure your authentication provider options here.
    // For more information, see https://aka.ms/blazor-standalone-auth
    builder.Configuration.Bind("Local", options.ProviderOptions);
});

Log.Debug("Running");
await builder.Build().RunAsync();
