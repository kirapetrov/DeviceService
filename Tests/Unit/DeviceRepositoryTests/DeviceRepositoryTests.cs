using DeviceRepository.Entities;
using DeviceRepository.Models;
using DeviceRepository.Repositories;
using MockQueryable.Moq;
using Moq;

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

    [Fact]
    public async void UpdateAsync_UpdateNullModel_EqualFlse()
    {
        var deviceContext = GetContextMock(Enumerable.Empty<Device>());
        var sut = new DeviceRepository.Repositories.DeviceRepository(deviceContext);
        var actual = await sut.UpdateAsync(1, null).ConfigureAwait(false);
        Assert.Equal(false, actual);
    }

    [Fact]
    public async void UpdateAsync_UpdateNotExistingModel_EqualFlse()
    {
        var deviceContext = GetContextMock(Enumerable.Empty<Device>());
        var deviceModel = new DeviceModel();
        var sut = new DeviceRepository.Repositories.DeviceRepository(deviceContext);
        var actual = await sut.UpdateAsync(1, deviceModel).ConfigureAwait(false);
        Assert.Equal(false, actual);
    }

    [Fact]
    public async void UpdateAsync_UpdateExistingModel_EqualTrue()
    {
        var deviceContext = GetContextMock(new List<Device> { new Device { Id = 1 } });
        var sut = new DeviceRepository.Repositories.DeviceRepository(deviceContext);
        var deviceModel = new DeviceModel();
        var actual = await sut.UpdateAsync(1, deviceModel).ConfigureAwait(false);
        Assert.Equal(true, actual);
    }

    [Fact]
    public async void DeleteAsync_DeleteNotExistingModel_EqualFlse()
    {
        var deviceContext = GetContextMock(Enumerable.Empty<Device>());
        var sut = new DeviceRepository.Repositories.DeviceRepository(deviceContext);
        var actual = await sut.DeleteAsync(1).ConfigureAwait(false);
        Assert.Equal(false, actual);
    }

    [Fact]
    public async void DeleteAsync_DeleteExistingModel_EqualTrue()
    {
        var deviceContext = GetContextMock(new List<Device> { new Device { Id = 1 } });
        var sut = new DeviceRepository.Repositories.DeviceRepository(deviceContext);
        var actual = await sut.DeleteAsync(1).ConfigureAwait(false);
        Assert.Equal(true, actual);
    }

    private DeviceContext GetContextMock(IEnumerable<Device> devices)
    {
        var deviceContextMock = new Mock<DeviceContext>();
        deviceContextMock
            .SetupGet(x => x.Devices)
            .Returns(devices.AsQueryable().BuildMockDbSet().Object);

        deviceContextMock
            .Setup(x => x.SaveChangesAsync(CancellationToken.None));

        deviceContextMock
            .Setup(x => x.Remove(It.IsAny<object>()));

        return deviceContextMock.Object;
    }
}