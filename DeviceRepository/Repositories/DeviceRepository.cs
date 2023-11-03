using DeviceRepository.Repositories.Interfaces;
using DeviceRepository.Models.Interfaces;
using DeviceRepository.Helpers;
using Microsoft.EntityFrameworkCore;
using DeviceRepository.Common;
using DeviceRepository.Common.Page;
using DeviceRepository.Common.Order;
using DeviceRepository.Common.Search;

namespace DeviceRepository.Repositories;

internal class DeviceRepository : IDeviceRepository
{
    private readonly DeviceContext dbContext;

    public DeviceRepository(DeviceContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<PagedResult<IDeviceModel>> GetAsync(
        QueryParameters? queryParameters,
        CancellationToken cancellationToken = default)
    {
        var orderInfo = queryParameters?.OrderInfo ?? new OrderInfo(nameof(IDeviceModel.Name));
        var deviceQuery = dbContext.Devices!
            .AsNoTracking()
            .AppendOrder(orderInfo)
            .AppendParameters(
                queryParameters?.SearchParameters ?? Enumerable.Empty<SearchParameter>().ToArray());

        var devicesCount = await deviceQuery
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);

        var pageInfo = queryParameters?.PageInfo ?? new PageInfo();
        var devices = await deviceQuery
            .AppendPage(pageInfo)
            .Select(x => x.GetModel())
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);

        return new PagedResult<IDeviceModel>(devices)
        {
            CurrentPage = pageInfo.Page,
            PageSize = pageInfo.Size,
            TotalCount = devicesCount,
            PageCount = (ushort)Math.Ceiling((double)devicesCount / pageInfo.Size)
        };
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