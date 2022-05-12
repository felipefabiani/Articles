using System.Collections;

namespace Articles.Test.Helper.Bases;

public abstract class TheoryData : IEnumerable<object[]>
{
    readonly List<object[]> _data = new();

    protected void AddRow(params object[] values)
    {
        _data.Add(values);
    }

    public IEnumerator<object[]> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class TheoryData<T> : TheoryData
    where T : notnull
{
    /// <summary>
    /// Adds data to the theory data set.
    /// </summary>
    /// <param name="p1">The first data value.</param>
    public void Add(T p1)
    {
        AddRow(p1);
    }

}