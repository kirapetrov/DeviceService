using DeviceRepository.Entities;
using DeviceRepository;
using MockQueryable.Moq;

namespace DeviceRepositoryTests;

public class DeviceRepositoryTests
{
    [Fact]
    public async void GetAsync_GetNotExistsModel_IsNull()
    {
        var deviceContext = GetContextMock(Enumerable.Empty<Device>());
        var sut = new DeviceRepository.Repositories.DeviceRepository(deviceContext);
        var actual = await sut.GetAsync(1).ConfigureAwait(false);
        Assert.Null(actual);
    }

    [Fact]
    public async void GetAsync_GetExistingModel_IsNotNull()
    {
        var deviceContext = GetContextMock(new List<Device> { new Device { Id = 1 } });
        var sut = new DeviceRepository.Repositories.DeviceRepository(deviceContext);
        var actual = await sut.GetAsync(1).ConfigureAwait(false);
        Assert.NotNull(actual);
    }

    private DeviceContext GetContextMock(IEnumerable<Device> devices)
    {
        return new DeviceContext
        {
            Devices = devices.AsQueryable().BuildMockDbSet().Object
        };
    }
}