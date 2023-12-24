using DeviceRepository.Devices.Interfaces;
using DeviceRepository.Tags.Interfaces;

namespace DeviceServiceTests.Mocks;

public class DeviceModelMock : IDeviceModel
{
    public long Identifier { get; set; }
    public string? Name { get; set; }
    public string? IpAddress { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public IReadOnlyCollection<ITagModel> Tags { get; } = [];
}