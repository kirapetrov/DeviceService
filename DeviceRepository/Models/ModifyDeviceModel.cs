using DeviceRepository.Models.Interfaces;

namespace DeviceRepository.Models;

public class ModifyDeviceModel : IModifyDeviceModel
{
    public string? Name { get; set; }
    public string? IpAddress { get; set; }
}