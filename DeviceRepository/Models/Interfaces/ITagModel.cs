namespace DeviceRepository.Models.Interfaces;

public interface ITagModel : IModelAdditionalInfo
{
    string? Name { get; }
    IReadOnlyCollection<IDeviceModel> Devices { get; }
}