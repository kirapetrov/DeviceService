using DeviceRepository.Common.Page;

public class PagedResult<T> : PagedResultBase where T : class
{
    public readonly IReadOnlyCollection<T> Results;

    public PagedResult(IReadOnlyCollection<T> results)
    {
        Results = results;
    }
}