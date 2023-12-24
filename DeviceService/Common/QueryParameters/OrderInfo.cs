using DeviceRepository.Common.Search;

namespace DeviceService.Common.QueryParameters;

public class OrderInfo
{
    public string? Name { get; set; }
    public OrderType OrderType { get; set; }
}