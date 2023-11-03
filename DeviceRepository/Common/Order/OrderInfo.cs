namespace DeviceRepository.Common.Order;

public class OrderInfo
{
    public string? Name { get; set; }
    public OrderType OrderType { get; set; }

    public OrderInfo() { }

    public OrderInfo(
        string columnName,
        OrderType orderType = OrderType.Ascending)
    {
        Name = columnName;
        OrderType = orderType;
    }
}