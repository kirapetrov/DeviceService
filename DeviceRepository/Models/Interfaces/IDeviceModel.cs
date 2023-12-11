namespace DeviceRepository.Models.Interfaces;

public interface IDeviceModel : IModelAdditionalInfo
{
    string? Name { get; }
    string? IpAddress { get; }
}