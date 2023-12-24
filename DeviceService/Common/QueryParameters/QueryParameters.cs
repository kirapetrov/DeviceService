namespace DeviceService.Common.QueryParameters;

public class QueryParameters
{
    public PageInfo? PageInfo { get; set; }

    public OrderInfo? OrderInfo { get; set; }

    public IReadOnlyCollection<SearchParameters>? SearchParameters { get; set; }
}