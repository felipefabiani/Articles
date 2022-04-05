namespace Articles.Api.Infrastructure;

public static class DecoratorFactory
{
    public static object? GetDecorator(Type type, object inner, IServiceProvider provider) => type switch
    {

        //_ when typeof(ILogingService).IsAssignableFrom(type) => new DecoratorLogingService(
        //    (ILogingService)inner, 
        //    (ILogger<DecoratorLogingService>)provider.GetRequiredService(typeof(ILogger<DecoratorLogingService>))),
        //_ when typeof(ILogingService).IsAssignableFrom(type) => new DecoratorLogingService(
        //    (ILogingService)inner, 
        //    (ILogger<DecoratorLogingService>)provider.GetRequiredService(typeof(ILogger<DecoratorLogingService>))),
        _ => null
    };
}
