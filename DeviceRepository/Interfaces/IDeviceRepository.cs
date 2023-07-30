using DataLayer.Interfaces;

namespace DeviceRepository.Interfaces;

public interface IDeviceRepository : IRepository
{
    Task<IEnumerable<IDeviceModel>> GetAsync();
    Task<IDeviceModel?> GetAsync(long identifier);
    Task<long> AddAsync(IDeviceModel deviceModel);
    Task<bool> UpdateAsync(long indentifier, IDeviceModel deviceModel);
    Task<bool> DeleteAsync(long identifier);
}