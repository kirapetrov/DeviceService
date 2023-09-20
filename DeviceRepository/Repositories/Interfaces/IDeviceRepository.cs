using DeviceRepository.Models.Interfaces;
using DeviceRepository.Common.Page;

namespace DeviceRepository.Repositories.Interfaces;

public interface IDeviceRepository
{
    Task<IEnumerable<IDeviceModel>> GetAsync(
        PageInfo pageInfo,
        CancellationToken cancellationToken = default);
    Task<IDeviceModel?> GetAsync(long identifier, CancellationToken cancellationToken = default);

    Task<long> AddAsync(IDeviceModel deviceModel, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(
        long identifier,
        IDeviceModel deviceModel,
        CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long identifier, CancellationToken cancellationToken = default);
}