using Moq;
using Microsoft.AspNetCore.Mvc;
using DeviceServiceTests.Mocks;
using DeviceService.Controllers;
using DeviceServiceTests.Helpers;
using DeviceService.Models;
using DeviceService.Page;
using DeviceRepository.Models.Interfaces;
using DeviceRepository.Repositories.Interfaces;
using DeviceService.QueryParameters;
using DeviceRepository.Common;

namespace DeviceServiceTests;

public class DeviceServiceTests
{
    [Fact]
    public async void GetDevices_GetMockDevices_DeviceCollectionsSame()
    {
        var expectedCollection = new IDeviceModel[] {
            new DeviceModelMock { Identifier = 1 },
            new DeviceModelMock { Identifier = 2 }
        };

        // Arrange
        var mockRepository = GetDeviceRepositoryMock(expectedCollection);
        var controller = new DeviceController(mockRepository.Object);

        // Act
        var actionResult = await controller.GetDevices(new QueryParameters());
        var actualCollection = actionResult.GetResult<OkObjectResult, PagedResult<Device>>();

        // Assert
        Assert.IsType<OkObjectResult>(actionResult?.Result);
        Assert.NotNull(actualCollection?.Collection);
        Assert.Equal(expectedCollection.Length, actualCollection.TotalCount);

        var index = 0;
        foreach (var actualItem in actualCollection.Collection)
        {
            Assert.NotNull(actualItem);
            Assert.Equal(expectedCollection[index].Identifier, actualItem.Identifier);
            index++;
        }
    }

    [Fact]
    public async void GetDevices_GetMockDevices_DeviceCollectionEmpty()
    {
        // Arrange
        var mockRepository = GetDeviceRepositoryMock([]);
        var controller = new DeviceController(mockRepository.Object);

        // Act
        var actionResult = await controller.GetDevices(new QueryParameters());
        var actionValue = actionResult.GetResult<OkObjectResult, PagedResult<Device>>();

        // Assert
        Assert.IsType<OkObjectResult>(actionResult?.Result);
        Assert.NotNull(actionValue);
        Assert.True(actionValue.TotalCount == 0);
    }

    private static Mock<IDeviceRepository> GetDeviceRepositoryMock(
        IDeviceModel[] devices)
    {
        var taskStub = Task.FromResult(new QueryResult<IDeviceModel>(devices, devices.Length));
        var mockRepository = new Mock<IDeviceRepository>();
        mockRepository
            .Setup(x => x.GetAsync(
                It.IsAny<string?>(),
                It.IsAny<OrderType>(),
                It.IsAny<ushort>(),
                It.IsAny<ushort>(),
                It.IsAny<IReadOnlyCollection<DeviceRepository.Common.SearchParameters>?>(),
                It.IsAny<CancellationToken>()))
            .Returns(taskStub);

        return mockRepository;
    }

    [Fact]
    public async void GetDevice_GetMockDeviceById_DevicesSame()
    {
        const long expectedIdentifier = 1;
        var expectedDevice = new DeviceModelMock { Identifier = expectedIdentifier };

        // Arrange
        var mockRepository = new Mock<IDeviceRepository>();
        mockRepository
            .Setup(x => x.GetAsync(expectedIdentifier, CancellationToken.None))
            .Returns(Task.FromResult<IDeviceModel?>(expectedDevice));

        var controller = new DeviceController(mockRepository.Object);

        // Act
        var actionResult = await controller.GetDevice(expectedIdentifier);
        var actionValue = actionResult.GetResult<OkObjectResult, Device>();

        // Assert
        Assert.IsType<OkObjectResult>(actionResult?.Result);
        Assert.NotNull(actionValue);
        Assert.Equal(expectedDevice.Identifier, actionValue.Identifier);
    }

    [Fact]
    public async void GetDevice_TryGetNonexistent_DeviceNull()
    {
        // Arrange
        var mockRepository = new Mock<IDeviceRepository>();
        var controller = new DeviceController(mockRepository.Object);

        // Act
        var actionResult = await controller.GetDevice(1);

        // Assert
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async void PostDevice_AddNewDevice_DeviceAdded()
    {
        var expected = new DeviceModelMock { Identifier = 1 };

        // Arrange
        var mockRepository = new Mock<IDeviceRepository>();
        mockRepository
            .Setup(x => x.AddAsync(1, It.IsAny<IModifyDeviceModel>(), CancellationToken.None))
            .Returns(Task.FromResult<IDeviceModel?>(expected));
        var controller = new DeviceController(mockRepository.Object);

        // Act
        var actionResult = await controller.PostDevice(GetEmptyDevice());
        var actionValue = actionResult.GetResult<CreatedAtActionResult, Device>();

        // Assert
        Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        Assert.NotNull(actionValue);
        Assert.Equal(expected.Identifier, actionValue.Identifier);
    }

    [Fact]
    public async void PostDevice_AddExistingDevice_DeviceNotAdded()
    {
        // Arrange
        var mockRepository = new Mock<IDeviceRepository>();
        var controller = new DeviceController(mockRepository.Object);

        // Act
        var actionResult = await controller.PostDevice(GetEmptyDevice());

        // Assert
        Assert.IsType<BadRequestResult>(actionResult.Result);
    }

    [Fact]
    public async void DeleteDevice_DeleteExistingDevice_DeviceDeleted()
    {
        const long expectedIdentifier = 1;

        // Arrange
        var mockRepository = new Mock<IDeviceRepository>();
        mockRepository
            .Setup(x => x.DeleteAsync(expectedIdentifier, CancellationToken.None))
            .Returns(Task.FromResult(true));
        var controller = new DeviceController(mockRepository.Object);

        // Act
        var actionResult = await controller
            .DeleteDevice(expectedIdentifier);

        // Assert
        Assert.IsType<NoContentResult>(actionResult);
    }

    [Fact]
    public async void DeleteDevice_DeleteNotExistingDevice_DeviceNotDeleted()
    {
        const long expectedIdentifier = 1;

        // Arrange
        var mockRepository = new Mock<IDeviceRepository>();
        mockRepository
            .Setup(x => x.DeleteAsync(expectedIdentifier, CancellationToken.None))
            .Returns(Task.FromResult(false));
        var controller = new DeviceController(mockRepository.Object);

        // Act
        var actionResult = await controller
            .DeleteDevice(expectedIdentifier);

        // Assert
        Assert.IsType<BadRequestResult>(actionResult);
    }

    private static Device GetEmptyDevice()
    {
        return new Device(
            1,
            DateTimeOffset.UtcNow,
            null,
            null,
            null,
            Array.Empty<Tag>()
        );
    }
}