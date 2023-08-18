namespace DeviceRepository.Models.Interfaces;

public interface IDeviceModel : IModelBase
{    
    string? Name { get; }
    string? IpAddress { get; }
}