namespace DeviceService.Page;

public class PagedResult<T>
{
    public ushort CurrentPage { get; set; } 
    public ushort PageCount { get; set; } 
    public ushort PageSize { get; set; } 
    public int TotalCount { get; set; }
    public IReadOnlyCollection<T>? Collection { get; set; }

    public int FirstRowOnPage => (CurrentPage - 1) * PageSize + 1; 
    public int LastRowOnPage => Math.Min(CurrentPage * PageSize, TotalCount);
}