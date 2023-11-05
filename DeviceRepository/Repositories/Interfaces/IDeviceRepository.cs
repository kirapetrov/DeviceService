using DeviceRepository.Models.Interfaces;
using DeviceRepository.Common;

namespace DeviceRepository.Repositories.Interfaces;

public interface IDeviceRepository
{
    Task<QueryResult<IDeviceModel>> GetAsync(
        string? orderProperty = null,
        OrderType orderType = OrderType.Ascending,
        ushort pageNumber = 0,
        ushort pageSize = 0,
        IReadOnlyCollection<SearchParameters>? searchParameters = null,
        CancellationToken cancellationToken = default);

    Task<IDeviceModel?> GetAsync(long identifier, CancellationToken cancellationToken = default);

    Task<long> AddAsync(IDeviceModel deviceModel, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(
        long identifier,
        IDeviceModel deviceModel,
        CancellationToken cancellationToken = default);
        
    Task<bool> DeleteAsync(long identifier, CancellationToken cancellationToken = default);
}