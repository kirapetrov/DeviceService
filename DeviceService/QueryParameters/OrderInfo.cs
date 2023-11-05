using DeviceRepository.Common;

namespace DeviceService.QueryParameters;

public class OrderInfo
{
    public string? Name { get; set; }
    public OrderType OrderType { get; set; }
}