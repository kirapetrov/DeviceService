using DeviceRepository.Devices.Interfaces;

namespace DeviceRepository.Devices;

public class ModifyDeviceModel : IModifyDeviceModel
{
    public string? Name { get; set; }
    public string? IpAddress { get; set; }
}