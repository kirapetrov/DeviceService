using DeviceRepository.Common.Models;
using DeviceRepository.Devices.Interfaces;
using DeviceRepository.Tags.Interfaces;

namespace DeviceRepository.Devices;

public class DeviceModel : ModelWithAdditionalInfo, IDeviceModel
{
    public string? Name { get; set; }
    public string? IpAddress { get; set; }

    public IReadOnlyCollection<ITagModel> Tags { get; set; } = [];
}