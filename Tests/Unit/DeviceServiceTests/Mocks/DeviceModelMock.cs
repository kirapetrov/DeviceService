using DeviceRepository.Models.Interfaces;

namespace DeviceServiceTests.Mocks;

public class DeviceModelMock : IDeviceModel
{
    public long Identifier { get; set; }

    public string? Name { get; set; }

    public string? IpAddress { get; set; }    
}