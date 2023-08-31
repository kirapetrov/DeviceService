using DeviceRepository.Repositories.Interfaces;
using DeviceRepository.Models.Interfaces;
using DeviceRepository.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DeviceRepository.Repositories;

internal class DeviceRepository : IDeviceRepository
{
    private readonly DeviceContext deviceContext;

    public DeviceRepository(DeviceContext deviceContext)
    {
        this.deviceContext = deviceContext;
    }

    public async Task<IEnumerable<IDeviceModel>> GetAsync()
    {
        return await deviceContext
            .Devices
            .Select(x => x.GetModel())
            .ToArrayAsync()
            .ConfigureAwait(false);
    }

    public async Task<IDeviceModel?> GetAsync(long identifier)
    {
        var device = await deviceContext
            .Devices
            .FirstOrDefaultAsync(x => x.Id == identifier)
            .ConfigureAwait(false);
        return device?.GetModel();
    }

    public async Task<long> AddAsync(IDeviceModel deviceModel)
    {
        var deviceEntity = deviceModel?.GetEntity();
        if (deviceEntity is null)
        {
            return -1;
        }

        var newDevice = await deviceContext
            .Devices
            .AddAsync(deviceEntity)
            .ConfigureAwait(false);
        await deviceContext.SaveChangesAsync().ConfigureAwait(false);
        return newDevice.Entity.Id;
    }

    public async Task<bool> UpdateAsync(long identifier, IDeviceModel deviceModel)
    {
        if (deviceModel is null)
        {
            return false;
        }

        var device = await deviceContext
            .Devices
            .FirstOrDefaultAsync(x => x.Id == identifier)
            .ConfigureAwait(false);

        if (device is null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(deviceModel.Name))
            device.Name = deviceModel.Name;

        if (!string.IsNullOrWhiteSpace(deviceModel.IpAddress))
            device.IpAddress = deviceModel.IpAddress;

        await deviceContext.SaveChangesAsync().ConfigureAwait(false);
        return true;
    }

    public async Task<bool> DeleteAsync(long identifier)
    {        
        var device = await deviceContext
            .Devices
            .FirstOrDefaultAsync(x => x.Id == identifier)
            .ConfigureAwait(false);

        if (device is null)
        {
            return false;
        }

        deviceContext.Remove(device);
        await deviceContext.SaveChangesAsync().ConfigureAwait(false);
        return true;
    }
}