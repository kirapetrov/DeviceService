using DeviceRepository.Common.Models.Interfaces;
using DeviceRepository.Devices.Interfaces;

namespace DeviceRepository.Tags.Interfaces;

public interface ITagModel : IModelAdditionalInfo
{
    string? Name { get; }
    IReadOnlyCollection<IDeviceModel> Devices { get; }
}