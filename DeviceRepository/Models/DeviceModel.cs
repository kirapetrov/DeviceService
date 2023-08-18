using DeviceRepository.Models.Interfaces;

namespace DeviceRepository.Models;

public class DeviceModel : IDeviceModel
{
    public long Identifier { get; set; }
    public string? Name { get; set; }
    public string? IpAddress { get; set; }
}
