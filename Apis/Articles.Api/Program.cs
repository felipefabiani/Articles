using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication
    .CreateBuilder(args)
    .AddServices();

var app = builder.Build();

_ = await app.SetApplication();

await app.RunAsync();
