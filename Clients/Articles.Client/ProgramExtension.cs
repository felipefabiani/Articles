using FluentValidation;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection;

namespace Articles.Client;
public static class WebAssemblyHostExtension
{
    public static void AddFluentValidators(this WebAssemblyHostBuilder builder, string assemblyName)
    {
        AddFluentValidators(builder, new List<string> { assemblyName });
    }
    public static void AddFluentValidators(this WebAssemblyHostBuilder builder, List<string> assemblyNames)
    {
        var validators = Assembly
            .GetEntryAssembly()!
            .GetReferencedAssemblies()
            .Where(assembly => assemblyNames.Any(name => assembly.FullName.Contains(name)))
            .Select(assembly => Assembly.Load(assembly))
            .SelectMany(assembly => assembly
                .GetTypes()
                .Where(t =>
                    !t.IsAbstract &&
                    t.BaseType is not null &&
                    t.BaseType.IsGenericType &&
                    t.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>)))
            .ToList();

        foreach (var validator in validators)
        {
            var arg = validator.BaseType!.GetGenericArguments().First();
            var baseType = typeof(AbstractValidator<>).MakeGenericType(arg);
            builder.Services.AddSingleton(baseType, validator);
        }
    }
}
