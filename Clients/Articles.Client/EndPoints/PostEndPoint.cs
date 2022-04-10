namespace Articles.Client.EndPoints;

public interface INotNullClass<T>
{
    T Empty { get; }
}

public interface IPostEndPoint<T, TResonse>
    where T : notnull, new()
    where TResonse : notnull, new()
{
    Dictionary<string, string> Headers { get; set; }
    T Model { get; set; }
    TResonse NullModel { get; }

    string GetEndPoint();
}

public abstract class PostEndPoint<T, TResonse> :
    IPostEndPoint<T, TResonse>
    where T : notnull, new()
    where TResonse : notnull, new()
{
    public T Model { get; set; } = new T();
    public abstract TResonse NullModel { get; }
    public abstract string GetEndPoint();

    public Dictionary<string, string> Headers { get; set; } = new();
}
