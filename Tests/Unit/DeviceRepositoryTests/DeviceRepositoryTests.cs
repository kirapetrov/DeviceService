using DeviceRepository.Devices;
using DeviceRepository.DataAccess;
using DeviceRepository.DataAccess.Entities;
using MockQueryable.Moq;
using Moq;

namespace DeviceRepositoryTests;

public class DeviceRepositoryTests
{
    private readonly Device deviceStub = new() { Id = 1 };

    [Fact]
    public async void GetAsync_GetNotExistsModel_IsNull()
    {
        var deviceContextMock = GetContextMock();
        var sut = new DeviceRepository.Devices.DeviceRepository(deviceContextMock.Object);
        var actual = await sut.GetAsync(1);
        Assert.Null(actual);
    }

    [Fact]
    public async void GetAsync_GetExistingModel_IsNotNull()
    {
        var deviceContextMock = GetContextMock(deviceStub);
        var sut = new DeviceRepository.Devices.DeviceRepository(deviceContextMock.Object);
        var actual = await sut.GetAsync(1);
        Assert.NotNull(actual);
    }

    [Fact (Skip = "How to mock EntityEntry")]
    public async void AddAsync_AddNotNullModel_EqualTrue()
    {
        var deviceContextMock = GetContextMock();
        var sut = new DeviceRepository.Devices.DeviceRepository(deviceContextMock.Object);
        var actual = await sut.AddAsync(1, new ModifyDeviceModel());
        Assert.NotNull(actual);
    }

    [Fact]
    public async void UpdateAsync_UpdateNotExistingModel_EqualFlse()
    {
        var deviceContextMock = GetContextMock();
        var sut = new DeviceRepository.Devices.DeviceRepository(deviceContextMock.Object);
        var actual = await sut.UpdateAsync(1, new ModifyDeviceModel());
        Assert.False(actual);
    }

    [Fact]
    public async void UpdateAsync_UpdateExistingModel_EqualTrue()
    {
        var deviceContextMock = GetContextMock(deviceStub);
        var sut = new DeviceRepository.Devices.DeviceRepository(deviceContextMock.Object);
        var actual = await sut.UpdateAsync(1, new ModifyDeviceModel());
        Assert.True(actual);
    }

    [Fact]
    public async void DeleteAsync_DeleteNotExistingModel_EqualFlse()
    {
        var deviceContextMock = GetContextMock();
        var sut = new DeviceRepository.Devices.DeviceRepository(deviceContextMock.Object);
        var actual = await sut.DeleteAsync(1);
        Assert.False(actual);
    }

    [Fact]
    public async void DeleteAsync_DeleteExistingModel_EqualTrue()
    {
        var deviceContextMock = GetContextMock(deviceStub);
        var sut = new DeviceRepository.Devices.DeviceRepository(deviceContextMock.Object);
        var actual = await sut.DeleteAsync(1);
        Assert.True(actual);
    }

    private Mock<DeviceContext> GetContextMock(Device? device = null)
    {
        var devices = device == null ?
            Enumerable.Empty<Device>() :
            new List<Device> { device };

        var deviceContextMock = new Mock<DeviceContext>();
        deviceContextMock
            .SetupGet(x => x.Devices)
            .Returns(devices.AsQueryable().BuildMockDbSet().Object);

        deviceContextMock
            .Setup(x => x.SaveChangesAsync(CancellationToken.None));

        if (device != null)
        {
            //TODO mock EntityEntry
            // var entryMock = new Mock<InternalEntityEntry>();
            // entryMock
            //     .SetupGet(x => x.Entity)
            //     .Returns(device);

            // deviceContextMock
            //     .Setup(x => x.Add(It.IsAny<object>()))
            //     .Returns(new EntityEntry(entryMock.Object));
        }

        deviceContextMock
            .Setup(x => x.Remove(It.IsAny<object>()));

        return deviceContextMock;
    }
}