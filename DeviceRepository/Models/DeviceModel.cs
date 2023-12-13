using DeviceRepository.Models.Interfaces;

namespace DeviceRepository.Models;

public class DeviceModel : ModelWithAdditionalInfo, IDeviceModel
{
    public string? Name { get; set; }
    public string? IpAddress { get; set; }

    public IReadOnlyCollection<ITagModel> Tags { get; set; } = [];
}