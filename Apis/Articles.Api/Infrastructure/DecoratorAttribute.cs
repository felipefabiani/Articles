namespace Articles.Api.Infrastructure;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DecoratorServiceAttribute : Attribute
{
    public Type Type { get; set; } = null!;
    public int Order { get; set; } = 0;
    public bool Skip { get; set; } = false;
}

public interface ITransientService { }
public interface IScopedService { }
public interface ISingletonService { }