using DeviceRepository.Models.Interfaces;

namespace DeviceRepository.Models;

public class TagModel : ModelWithAdditionalInfo, ITagModel
{
    public string? Name { get; set; }

    public IReadOnlyCollection<IDeviceModel> Devices { get; set; } = [];
}