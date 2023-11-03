using DeviceRepository.Models.Interfaces;
using DeviceRepository.Common;

namespace DeviceRepository.Repositories.Interfaces;

public interface IDeviceRepository
{
    Task<PagedResult<IDeviceModel>> GetAsync(
        QueryParameters? queryParameters,
        CancellationToken cancellationToken = default);
    Task<IDeviceModel?> GetAsync(long identifier, CancellationToken cancellationToken = default);

    Task<long> AddAsync(IDeviceModel deviceModel, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(
        long identifier,
        IDeviceModel deviceModel,
        CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long identifier, CancellationToken cancellationToken = default);
}