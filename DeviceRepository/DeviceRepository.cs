using DeviceRepository.Interfaces;
using DeviceRepository.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DeviceRepository;

internal class DeviceRepository : IDeviceRepository
{
    private readonly IDbContext dbContext;

    public DeviceRepository(IDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<IDeviceModel>> GetAsync()
    {
        return await dbContext.Devices
            .Select(x => x.GetModel())
            .ToArrayAsync()
            .ConfigureAwait(false);
    }

    public async Task<IDeviceModel?> GetAsync(long identifier)
    {
        var device = await dbContext.Devices
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        return device?.GetModel();
    }

    public async Task<long> AddAsync(IDeviceModel deviceModel)
    {
        var deviceEntity = deviceModel?.GetEntity();
        if (deviceEntity == null)
        {
            return -1;
        }

        var newDevice = await dbContext
            .AddAsync(deviceEntity)
            .ConfigureAwait(false);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);
        return newDevice.Entity.Id;
    }

    public async Task<bool> UpdateAsync(long identifier, IDeviceModel deviceModel)
    {
        if (deviceModel == null)
        {
            return false;
        }

        var device = await dbContext.Devices
            .FirstOrDefaultAsync(x => x.Id == identifier)
            .ConfigureAwait(false);

        if (device == null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(deviceModel.Name))
            device.Name = deviceModel.Name;

        if (!string.IsNullOrWhiteSpace(deviceModel.IpAddress))
            device.IpAddress = deviceModel.IpAddress;

        await dbContext.SaveChangesAsync().ConfigureAwait(false);
        return true;
    }

    public async Task<bool> DeleteAsync(long identifier)
    {        
        var device = await dbContext.Devices
            .FirstOrDefaultAsync(x => x.Id == identifier)
            .ConfigureAwait(false);

        if (device == null)
        {
            return false;
        }
        dbContext.Remove(device);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public void Dispose()
    {
        dbContext.Dispose();
    }
}