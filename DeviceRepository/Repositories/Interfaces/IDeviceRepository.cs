using DeviceRepository.Models.Interfaces;

namespace DeviceRepository.Repositories.Interfaces;

public interface IDeviceRepository
{
    Task<IEnumerable<IDeviceModel>> GetAsync(CancellationToken cancellationToken = default);
    Task<IDeviceModel?> GetAsync(long identifier, CancellationToken cancellationToken = default);

    Task<long> AddAsync(IDeviceModel deviceModel, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(
        long identifier,
        IDeviceModel deviceModel,
        CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long identifier, CancellationToken cancellationToken = default);
}