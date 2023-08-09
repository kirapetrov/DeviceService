namespace DeviceRepository.Interfaces;

public interface IDeviceModel
{
    long Identifier { get; }
    string? Name { get; }
    string? IpAddress { get; }
}