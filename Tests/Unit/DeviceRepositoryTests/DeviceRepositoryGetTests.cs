using DeviceRepository.DataAccess;
using DeviceRepository.DataAccess.Entities;
using MockQueryable.Moq;
using Moq;

namespace DeviceRepositoryTests;

public class DeviceRepositoryGetTests
{
    public void GetAsync_GetWithoutParametes_PageResultWihDefaultParameters()
    {
    }

    private Mock<DeviceContext> GetContextMock(Device? device = null)
    {
        var deviceContextMock = new Mock<DeviceContext>();
        deviceContextMock
            .SetupGet(x => x.Devices)
            .Returns(Enumerable.Empty<Device>().AsQueryable().BuildMockDbSet().Object);

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