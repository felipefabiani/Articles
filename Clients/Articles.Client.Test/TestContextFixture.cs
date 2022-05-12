using Articles.Client;
using Articles.Client.Authentication;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace Articles.Client.Test
{
    public class TestContextFixture : IDisposable
    {
        private TestContext _ctx;

        public TestContext Context { get => _ctx; }

        public TestContextFixture()
        {
            _ctx = new TestContext();
            _ctx.Services.AddOptions();
            _ctx.Services.AddAuthorizationCore();
            _ctx.Services.AddScoped<IAuthStateProvider, FakeAuthStateProvider>();
            _ctx.Services.AddScoped<AuthenticationStateProvider>(p => (FakeAuthStateProvider)p.GetRequiredService<IAuthStateProvider>());
            // _ctx.Services.AddBlazoredLocalStorage();
            _ctx.Services.AddScoped<ILocalStorageService, FakeLocalStorageService>();
            _ctx.Services.AddMudServices(ProgramExtension.MudConfig);
            _ctx.Services.AddTransient<IAuthenticationService, AuthenticationService>();
            AddValidators();

            void AddValidators()
            {
                var assembly = Directory
                    .GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Articles.*.dll")
                    .Select(file => Assembly.LoadFrom(file))
                    .ToList();

                var validators = assembly.GetValidators();
                foreach (var validator in validators)
                {
                    var arg = validator.BaseType!.GetGenericArguments().First();
                    var baseType = typeof(AbstractValidator<>).MakeGenericType(arg);
                    _ctx.Services.AddSingleton(baseType, validator);
                }
            }
        }

        public void AddValidator<T>(T request)
            where T : class
        {
            var arg = request.GetType().BaseType!.GetGenericArguments().First();
            var baseType = typeof(AbstractValidator<>).MakeGenericType(arg);
            _ctx.Services.AddSingleton(baseType, request);
        }

        public void AddHttpClient<T>(
            T response,
            string endpoint = "/",
            HttpStatusCode statusCode = HttpStatusCode.OK,
            string baseAddress = "http://localhost:8080")
        {
            try
            {
                _ctx.Services.AddSingleton<IHttpClientFactory>(
                       new FakeHttpClientFactory<T>(
                          response: response,
                          endpoint: endpoint,
                          statusCode: statusCode,
                          baseAddress: baseAddress));
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void Dispose()
        {
            _ctx?.Dispose();
        }
    }
}
