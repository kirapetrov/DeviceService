namespace DeviceRepository.Common.Page;

public abstract class PagedResultBase
{
    public ushort CurrentPage { get; set; } 
    public ushort PageCount { get; set; } 
    public ushort PageSize { get; set; } 
    public int TotalCount { get; set; }
 
    public int FirstRowOnPage => (CurrentPage - 1) * PageSize + 1;
 
    public int LastRowOnPage => Math.Min(CurrentPage * PageSize, TotalCount);
}