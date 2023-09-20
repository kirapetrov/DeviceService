using DeviceRepository.Common.Page;

public class PagedResult<T> : PagedResultBase where T : class
{
    public readonly IQueryable<T> Results;

    public PagedResult(IQueryable<T> results)
    {
        Results = results;
    }
}