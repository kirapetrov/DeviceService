using Microsoft.EntityFrameworkCore;
using DeviceRepository.Helpers;
using DeviceRepository.Common;
using DeviceRepository.DataAccess;
using DeviceRepository.Devices.Interfaces;
using DeviceRepository.Common.Search;

namespace DeviceRepository.Devices;

internal class DeviceRepository : IDeviceRepository
{
    private readonly DeviceContext dbContext;

    public DeviceRepository(DeviceContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<QueryResult<IDeviceModel>> GetAsync(
        string? orderProperty = null,
        OrderType orderType = OrderType.Ascending,
        ushort pageNumber = 0,
        ushort pageSize = 0,
        IReadOnlyCollection<SearchParameters>? searchParameters = null,
        CancellationToken cancellationToken = default)
    {
        var deviceQuery = dbContext.Devices!
            .AsNoTracking()
            .AppendOrder(orderProperty, orderType)
            .AppendParameters(searchParameters);

        var devicesCount = await deviceQuery
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);

        var devices = await deviceQuery
            .AppendPage(pageNumber, pageSize)
            .Select(x => x.GetModel(true))
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);
        
        return new QueryResult<IDeviceModel>(devices, devicesCount);
    }

    public async Task<IDeviceModel?> GetAsync(long identifier, CancellationToken cancellationToken = default)
    {
        var device = await dbContext
            .Devices!
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == identifier,
                cancellationToken)
            .ConfigureAwait(false);
        return device?.GetModel(true);
    }

    public async Task<IDeviceModel?> AddAsync(
        long userId,
        IModifyDeviceModel modifyDeviceModel,
        CancellationToken cancellationToken = default)
    {
        var deviceEntity = modifyDeviceModel?.GetEntity(userId);
        if (deviceEntity is null)
        {
            return null;
        }

        var newDevice = await dbContext
            .Devices!
            .AddAsync(deviceEntity, cancellationToken)
            .ConfigureAwait(false);

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return newDevice.Entity.GetModel(true);
    }

    public async Task<bool> UpdateAsync(
        long deviceIdentifier,
        IModifyDeviceModel modifyDeviceModel,
        CancellationToken cancellationToken = default)
    {
        if (modifyDeviceModel is null)
        {
            return false;
        }

        var device = await dbContext
            .Devices!
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == deviceIdentifier,
                cancellationToken)
            .ConfigureAwait(false);

        if (device is null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(modifyDeviceModel.Name))
            device.Name = modifyDeviceModel.Name;

        if (!string.IsNullOrWhiteSpace(modifyDeviceModel.IpAddress))
            device.IpAddress = modifyDeviceModel.IpAddress;

        device.UpdatedAt = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    public async Task<bool> DeleteAsync(long identifier, CancellationToken cancellationToken = default)
    {
        var device = await dbContext
            .Devices!
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == identifier,
                cancellationToken)
            .ConfigureAwait(false);

        if (device is null)
        {
            return false;
        }

        dbContext.Remove(device);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }
}