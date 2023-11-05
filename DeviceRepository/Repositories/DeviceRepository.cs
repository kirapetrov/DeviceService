using Microsoft.EntityFrameworkCore;
using DeviceRepository.Repositories.Interfaces;
using DeviceRepository.Models.Interfaces;
using DeviceRepository.Helpers;
using DeviceRepository.Common;

namespace DeviceRepository.Repositories;

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
            .Select(x => x.GetModel())
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
        return device?.GetModel();
    }

    public async Task<long> AddAsync(IDeviceModel deviceModel, CancellationToken cancellationToken = default)
    {
        var deviceEntity = deviceModel?.GetEntity();
        if (deviceEntity is null)
        {
            return -1;
        }

        var newDevice = await dbContext
            .Devices!
            .AddAsync(deviceEntity, cancellationToken)
            .ConfigureAwait(false);

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return newDevice.Entity.Id;
    }

    public async Task<bool> UpdateAsync(
        long identifier,
        IDeviceModel deviceModel,
        CancellationToken cancellationToken = default)
    {
        if (deviceModel is null)
        {
            return false;
        }

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

        if (!string.IsNullOrWhiteSpace(deviceModel.Name))
            device.Name = deviceModel.Name;

        if (!string.IsNullOrWhiteSpace(deviceModel.IpAddress))
            device.IpAddress = deviceModel.IpAddress;

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