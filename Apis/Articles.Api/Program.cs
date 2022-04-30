using Microsoft.AspNetCore.Builder;

var builder = WebApplication
    .CreateBuilder(args)
    .AddServices();

var app = builder.Build();

_ = await app.SetApplication();

await app.RunAsync();
