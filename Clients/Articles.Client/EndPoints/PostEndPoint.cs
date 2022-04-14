using CSharpFunctionalExtensions;

namespace Articles.Client.EndPoints;

public interface INotNullClass<T>
{
    T Empty { get; }
}

public interface IPostEndPoint<TRequest, TResponse>
    where TRequest : notnull, new()
    where TResponse : notnull, new()
{
    Dictionary<string, string> Headers { get; set; }
    TRequest Model { get; set; }
    TResponse Response { get; set; }

    string GetEndPoint();
}

public abstract class PostEndPoint<TRequest, TResponse> :
    IPostEndPoint<TRequest, TResponse>
    where TRequest : notnull, new()
    where TResponse : notnull, new()
{
    public TRequest Model { get; set; } = new TRequest();

    public TResponse Response { get; set; }
    public abstract string GetEndPoint();

    public Dictionary<string, string> Headers { get; set; } = new();
}
