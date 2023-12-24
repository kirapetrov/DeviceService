using DeviceRepository.Common.Models;
using DeviceRepository.Devices.Interfaces;
using DeviceRepository.Tags.Interfaces;

namespace DeviceRepository.Tags;

public class TagModel : ModelWithAdditionalInfo, ITagModel
{
    public string? Name { get; set; }

    public IReadOnlyCollection<IDeviceModel> Devices { get; set; } = [];
}