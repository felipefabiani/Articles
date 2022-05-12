

namespace Articles.Client.Test;

public class FakeLocalStorageService : ILocalStorageService
{
    public event EventHandler<ChangingEventArgs>? Changing;
    public event EventHandler<ChangedEventArgs>? Changed;

    private Dictionary<string, string> _keys = new()
    {
        {"userId", "2" }
    };

    public ValueTask ClearAsync(CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> ContainKeyAsync(string key, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public ValueTask<string> GetItemAsStringAsync(string key, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public ValueTask<T> GetItemAsync<T>(string key, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public ValueTask<string> KeyAsync(int index, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IEnumerable<string>> KeysAsync(CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public ValueTask<int> LengthAsync(CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public ValueTask RemoveItemAsync(string key, CancellationToken? cancellationToken = null)
    {
        if (_keys.ContainsKey(key))
        {
            _keys.Remove(key);
        }
        return ValueTask.CompletedTask;
    }

    public ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public ValueTask SetItemAsStringAsync(string key, string data, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public ValueTask SetItemAsync<T>(string key, T data, CancellationToken? cancellationToken = null)
    {
        var jsonData = JsonSerializer.Serialize(data);
        if (_keys.ContainsKey(key))
        {
            _keys[key] = jsonData;
        }
        else
        {
            _keys.Add(key, jsonData);
        }

        return ValueTask.CompletedTask;
    }
}
