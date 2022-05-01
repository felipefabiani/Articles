using AutoFixture.Dsl;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Articles.Test.Helper.Extensions;

[ExcludeFromCodeCoverage]
public static class AutoFixtureExtension
{
    private readonly static Random _random = new();
    public static IPostprocessComposer<T> WithStringLength<T>(this ICustomizationComposer<T> build, Expression<Func<T, string>> propertyPicker, int length)
    {
        return WithStringLengthPrivate(build, propertyPicker, length);
    }

    public static IPostprocessComposer<T> WithStringLength<T>(this IPostprocessComposer<T> build, Expression<Func<T, string>> propertyPicker, int length)
    {
        return WithStringLengthPrivate(build, propertyPicker, length);
    }

    private static IPostprocessComposer<T> WithStringLengthPrivate<T>(IPostprocessComposer<T> build, Expression<Func<T, string>> propertyPicker, int length)
    {
        return build.With(propertyPicker, RandomString(length));
    }
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable
            .Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)])
            .ToArray());
    }
}
