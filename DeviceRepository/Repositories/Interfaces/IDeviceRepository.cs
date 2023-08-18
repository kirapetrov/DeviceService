using DeviceRepository.Models.Interfaces;

namespace DeviceRepository.Repositories.Interfaces;

public interface IDeviceRepository
{
    Task<IEnumerable<IDeviceModel>> GetAsync();
    Task<IDeviceModel?> GetAsync(long identifier);

    Task<long> AddAsync(IDeviceModel deviceModel);
    Task<bool> UpdateAsync(long identifier, IDeviceModel deviceModel);
    Task<bool> DeleteAsync(long identifier);
}