namespace DeviceRepository.Common.Page;

public class PageInfo
{
    public ushort Size { get; set; }
    public ushort Page { get; set; }

    public PageInfo() : this(20, 1)
    {
    }

    public PageInfo(ushort size, ushort page)
    {
        Size = size;
        Page = page;
    }
}