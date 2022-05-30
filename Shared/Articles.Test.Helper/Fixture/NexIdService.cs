using System.Collections.Concurrent;

namespace Articles.Test.Helper.Fixture;
public class NexIdService
{
    private readonly ConcurrentQueue<int> _NextIdQueue;

    public NexIdService()
    {
        _NextIdQueue = new ConcurrentQueue<int>();
        // Populate the queue.
        for (int i = 10_000; i <= 100_000; i++)
        {
            _NextIdQueue.Enqueue(i);
        }
    }

    public int GetNextId()
    {
        int id;
        while (!_NextIdQueue.TryDequeue(out id)) { }
        return id;
    }
}
