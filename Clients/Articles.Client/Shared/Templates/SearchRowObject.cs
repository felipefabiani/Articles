namespace Articles.Client.Shared.Templates;

public class Table<T>
{
    public string? Title { get; set; }
    public FilterFunc? Filter { get; set; } = null;

    public delegate bool FilterFunc(T element, string str);
    public List<SearchRowObject<T>> Columns { get; set; } = default!;
}
public class SearchRowObject<T>
{
    public string HeaderName { get; set; } = default!;
    public Value RowValue { get; set; } = default!;

    public delegate string Value(T str);
}

public class DataGrid<T>
{
    public string? Title { get; set; }
    public FilterFunc? Filter { get; set; } = null;

    public delegate bool FilterFunc(T element, string str);
    public List<DataColumn<T>> Columns { get; set; } = default!;
}

public class DataColumn<T>
{
    public string Title { get; set; } = default!;
    public string Field { get; set; } = default!;
    public bool Sortable { get; set; } = false;
    public bool Filterable { get; set; } = true;
    public Func<T, object> SortBy { get; set; } = default!;
    public Func<T, string> CellStyleFunc { get; set; } = default!;
    public bool HasCellTemplate { get; set; } = false;
    public Func<T, string>? CellTemplate { get; set; } = null;
}