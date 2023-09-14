using DeviceRepository.Entities;
using DeviceRepository.Models;
using DeviceRepository.Repositories;
using MockQueryable.Moq;
using Moq;

namespace DeviceRepositoryTests;

public class DeviceRepositoryTests
{
    private readonly Device deviceStub = new Device { Id = 1 };

    [Fact]
    public async void GetAsync_GetNotExistsModel_IsNull()
    {
        var deviceContextMock = GetContextMock();
        var sut = new DeviceRepository.Repositories.DeviceRepository(deviceContextMock.Object);
        var actual = await sut.GetAsync(1).ConfigureAwait(false);
        Assert.Null(actual);
    }

    [Fact]
    public async void GetAsync_GetExistingModel_IsNotNull()
    {
        var deviceContextMock = GetContextMock(deviceStub);
        var sut = new DeviceRepository.Repositories.DeviceRepository(deviceContextMock.Object);
        var actual = await sut.GetAsync(1).ConfigureAwait(false);
        Assert.NotNull(actual);
    }

    [Fact]
    public async void AddAsync_AddNullModel_EqualFlse()
    {
        var deviceContextMock = GetContextMock();
        var sut = new DeviceRepository.Repositories.DeviceRepository(deviceContextMock.Object);
        var actual = await sut.AddAsync(null).ConfigureAwait(false);
        Assert.Equal(-1, actual);
    }

    [Fact (Skip = "How to mock EntityEntry")]
    public async void AddAsync_AddNotNullModel_EqualTrue()
    {
        var deviceContextMock = GetContextMock();
        var sut = new DeviceRepository.Repositories.DeviceRepository(deviceContextMock.Object);
        var actual = await sut.AddAsync(new DeviceModel()).ConfigureAwait(false);
        Assert.Equal(1, actual);
    }

    [Fact]
    public async void UpdateAsync_UpdateNullModel_EqualFlse()
    {
        var deviceContextMock = GetContextMock();
        var sut = new DeviceRepository.Repositories.DeviceRepository(deviceContextMock.Object);
        var actual = await sut.UpdateAsync(1, null).ConfigureAwait(false);
        Assert.False(actual);
    }

    [Fact]
    public async void UpdateAsync_UpdateNotExistingModel_EqualFlse()
    {
        var deviceContextMock = GetContextMock();
        var deviceModel = new DeviceModel();
        var sut = new DeviceRepository.Repositories.DeviceRepository(deviceContextMock.Object);
        var actual = await sut.UpdateAsync(1, deviceModel).ConfigureAwait(false);
        Assert.False(actual);
    }

    [Fact]
    public async void UpdateAsync_UpdateExistingModel_EqualTrue()
    {
        var deviceContextMock = GetContextMock(deviceStub);
        var sut = new DeviceRepository.Repositories.DeviceRepository(deviceContextMock.Object);
        var deviceModel = new DeviceModel();
        var actual = await sut.UpdateAsync(1, deviceModel).ConfigureAwait(false);
        Assert.True(actual);
    }

    [Fact]
    public async void DeleteAsync_DeleteNotExistingModel_EqualFlse()
    {
        var deviceContextMock = GetContextMock();
        var sut = new DeviceRepository.Repositories.DeviceRepository(deviceContextMock.Object);
        var actual = await sut.DeleteAsync(1).ConfigureAwait(false);
        Assert.False(actual);
    }

    [Fact]
    public async void DeleteAsync_DeleteExistingModel_EqualTrue()
    {
        var deviceContextMock = GetContextMock(deviceStub);
        var sut = new DeviceRepository.Repositories.DeviceRepository(deviceContextMock.Object);
        var actual = await sut.DeleteAsync(1).ConfigureAwait(false);

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