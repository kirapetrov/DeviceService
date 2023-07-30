using Moq;
using Microsoft.AspNetCore.Mvc;
using DeviceServiceTests.Mocks;
using DeviceRepository.Interfaces;
using DeviceService.Controllers;
using DeviceServiceTests.Helpers;
using DeviceService.Models;

namespace DeviceServiceTests;

public class DeviceServiceTests
{
    [Fact]
    public async void GetDevices_GetMockDevices_DeviceCollectionsSame()
    {
        var expectedCollection = new[] {
            new DeviceModelMock { Identifier = 1 },
            new DeviceModelMock { Identifier = 2 }};

        // Arrange
        var mockRepository = new Mock<IDeviceRepository>();
        mockRepository
            .Setup(x => x.GetAsync())
            .Returns(Task.FromResult<IEnumerable<IDeviceModel>>(expectedCollection));

        var controller = new DeviceController(mockRepository.Object);

        // Act
        var actionResult = await controller.GetDevices().ConfigureAwait(false);
        var actualCollection = actionResult
            .GetResult<OkObjectResult, IEnumerable<Device>>()?
            .ToArray();

        // Assert
        Assert.IsType<OkObjectResult>(actionResult?.Result);
        Assert.NotNull(actualCollection);
        Assert.Equal(expectedCollection.Length, actualCollection.Length);

        var index = 0;
        while(index < expectedCollection.Length){
            var expectedItem = expectedCollection[index];
            var actualItem = actualCollection[index];

            Assert.NotNull(actualItem);
            Assert.Equal(expectedItem.Identifier, actualItem.Identifier);
            index++;
        }
    }

    [Fact]
    public async void GetDevices_GetMockDevices_DeviceCollectionEmpty()
    {
        // Arrange
        var mockRepository = new Mock<IDeviceRepository>();
        mockRepository
            .Setup(x => x.GetAsync())
            .Returns(Task.FromResult(Enumerable.Empty<IDeviceModel>()));

        var controller = new DeviceController(mockRepository.Object);

        // Act
        var actionResult = await controller.GetDevices().ConfigureAwait(false);
        var actionValue = actionResult.GetResult<OkObjectResult, IEnumerable<Device>>();

        // Assert
        Assert.IsType<OkObjectResult>(actionResult?.Result);
        Assert.NotNull(actionValue);
        Assert.True(actionValue.Count() == 0);
    }

    [Fact]
    public async void GetDevice_GetMockDeviceById_DevicesSame()
    {
        const long expectedIdentifier = 1;
        DeviceModelMock expectedDevice = new DeviceModelMock { Identifier = expectedIdentifier };

        // Arrange
        var mockRepository = new Mock<IDeviceRepository>();
        mockRepository
            .Setup(x => x.GetAsync(expectedIdentifier))
            .Returns(Task.FromResult<IDeviceModel?>(expectedDevice));

        var controller = new DeviceController(mockRepository.Object);

        // Act
        var actionResult = await controller
            .GetDevice(expectedIdentifier)
            .ConfigureAwait(false);
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
        var actionResult = await controller
            .GetDevice(1)
            .ConfigureAwait(false);

        // Assert
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async void PostDevice_AddNewDevice_DeviceAdded()
    {
        const long expectedIdentifier = 1;

        // Arrange
        var mockRepository = new Mock<IDeviceRepository>();
        mockRepository
            .Setup(x => x.AddAsync(It.IsAny<IDeviceModel>()))
            .Returns(Task.FromResult(expectedIdentifier));
        var controller = new DeviceController(mockRepository.Object);

        // Act
        var actionResult = await controller
            .PostDevice(new Device(0, null, null))
            .ConfigureAwait(false);
        var actionValue = actionResult.GetResult<CreatedAtActionResult, Device>();

        // Assert
        Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        Assert.NotNull(actionValue);
        Assert.Equal(expectedIdentifier, actionValue.Identifier);
    }

    [Fact]
    public async void PostDevice_AddExistingDevice_DeviceNotAdded()
    {
        // Arrange
        var mockRepository = new Mock<IDeviceRepository>();
        var controller = new DeviceController(mockRepository.Object);

        // Act
        var actionResult = await controller
            .PostDevice(new Device(0, null, null))
            .ConfigureAwait(false);

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
            .Setup(x => x.DeleteAsync(expectedIdentifier))
            .Returns(Task.FromResult(true));
        var controller = new DeviceController(mockRepository.Object);

        // Act
        var actionResult = await controller
            .DeleteDevice(expectedIdentifier)
            .ConfigureAwait(false);

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
            .Setup(x => x.DeleteAsync(expectedIdentifier))
            .Returns(Task.FromResult(false));
        var controller = new DeviceController(mockRepository.Object);

        // Act
        var actionResult = await controller
            .DeleteDevice(expectedIdentifier)
            .ConfigureAwait(false);

        // Assert
        Assert.IsType<BadRequestResult>(actionResult);
    }
}