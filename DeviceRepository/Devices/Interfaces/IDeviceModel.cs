using DeviceRepository.Common.Models.Interfaces;
using DeviceRepository.Tags.Interfaces;

namespace DeviceRepository.Devices.Interfaces;

public interface IDeviceModel : IModelAdditionalInfo
{
    string? Name { get; }
    string? IpAddress { get; }
    IReadOnlyCollection<ITagModel> Tags { get; }
}