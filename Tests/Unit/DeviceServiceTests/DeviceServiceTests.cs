using Moq;
using Microsoft.AspNetCore.Mvc;
using DeviceServiceTests.Mocks;
using DeviceService.Controllers;
using DeviceServiceTests.Helpers;
using DeviceService.Models;
using DeviceRepository.Common.Page;
using DeviceRepository.Models.Interfaces;
using DeviceRepository.Repositories.Interfaces;
using DeviceRepository.Common;

namespace DeviceServiceTests;

public class DeviceServiceTests
{
    [Fact]
    public async void GetDevices_GetMockDevices_DeviceCollectionsSame()
    {
        var queryParameters = new QueryParameters();
        var expectedCollection = new[] {
            new DeviceModelMock { Identifier = 1 },
            new DeviceModelMock { Identifier = 2 }};

        // Arrange
        var mockRepository = new Mock<IDeviceRepository>();
        mockRepository
            .Setup(x => x.GetAsync(queryParameters, CancellationToken.None))
            .Returns(GetPagedResult(expectedCollection, 1, 1));

        var controller = new DeviceController(mockRepository.Object);

        // Act
        var actionResult = await controller.GetDevices(queryParameters).ConfigureAwait(false);
        var actualCollection = actionResult
            .GetResult<OkObjectResult, PagedResult<Device>>();

        // Assert
        Assert.IsType<OkObjectResult>(actionResult?.Result);
        Assert.NotNull(actualCollection);
        Assert.Equal(expectedCollection.Length, actualCollection.TotalCount);

        var index = 0;
        foreach (var actualItem in actualCollection.Results)
        {
            Assert.NotNull(actualItem);
            Assert.Equal(expectedCollection[index].Identifier, actualItem.Identifier);
            index++;
        }
    }

    [Fact]
    public async void GetDevices_GetMockDevices_DeviceCollectionEmpty()
    {
        var queryParameters = new QueryParameters();
        // Arrange
        var mockRepository = new Mock<IDeviceRepository>();
        mockRepository
            .Setup(x => x.GetAsync(queryParameters, CancellationToken.None))
            .Returns(GetPagedResult(Enumerable.Empty<IDeviceModel>()));

        var controller = new DeviceController(mockRepository.Object);

        // Act
        var actionResult = await controller.GetDevices(queryParameters).ConfigureAwait(false);
        var actionValue = actionResult.GetResult<OkObjectResult, PagedResult<Device>>();

        // Assert
        Assert.IsType<OkObjectResult>(actionResult?.Result);
        Assert.NotNull(actionValue);
        Assert.True(actionValue.TotalCount == 0);
    }

    private Task<PagedResult<IDeviceModel>> GetPagedResult(
        IEnumerable<IDeviceModel> devices,
        ushort page = 1,
        ushort size = 1)
    {
        var deviceCollection = devices.ToArray();
        var itemsCount = deviceCollection.Length;
        var pageCount = (ushort)Math.Ceiling((double)itemsCount / size);
        var pagedResult = new PagedResult<IDeviceModel>(deviceCollection)
        {
            CurrentPage = page,
            PageSize = size,
            TotalCount = itemsCount,
            PageCount = pageCount
        };

        return Task.FromResult(pagedResult);
    }

    [Fact]
    public async void GetDevice_GetMockDeviceById_DevicesSame()
    {
        const long expectedIdentifier = 1;
        DeviceModelMock expectedDevice = new DeviceModelMock { Identifier = expectedIdentifier };

        // Arrange
        var mockRepository = new Mock<IDeviceRepository>();
        mockRepository
            .Setup(x => x.GetAsync(expectedIdentifier, CancellationToken.None))
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
            .Setup(x => x.AddAsync(It.IsAny<IDeviceModel>(), CancellationToken.None))
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
            .Setup(x => x.DeleteAsync(expectedIdentifier, CancellationToken.None))
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
            .Setup(x => x.DeleteAsync(expectedIdentifier, CancellationToken.None))
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