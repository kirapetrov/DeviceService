using DeviceRepository.Common.Page;

public class PagedResult<T> : PagedResultBase where T : class
{
    public IReadOnlyCollection<T> Results { get; set; }

    public PagedResult(IReadOnlyCollection<T> results)
    {
        Results = results;
    }
}