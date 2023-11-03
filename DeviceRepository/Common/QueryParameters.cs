using DeviceRepository.Common.Order;
using DeviceRepository.Common.Page;
using DeviceRepository.Common.Search;

namespace DeviceRepository.Common;

public class QueryParameters
{
    public PageInfo? PageInfo { get; set; }

    public OrderInfo? OrderInfo { get; set; }

    public IReadOnlyCollection<SearchParameter>? SearchParameters { get; set; }
}