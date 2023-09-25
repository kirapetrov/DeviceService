namespace DeviceRepository.Common.Page;

public class PageInfo
{
    public int Size { get; set; }
    public int Page { get; set; }

    public PageInfo() : this(20, 1)
    {
    }

    public PageInfo(int size, int page)
    {
        Size = size;
        Page = page;
    }
}