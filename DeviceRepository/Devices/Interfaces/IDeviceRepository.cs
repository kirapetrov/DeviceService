using DeviceRepository.Common;
using DeviceRepository.Common.Search;

namespace DeviceRepository.Devices.Interfaces;

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

    Task<IDeviceModel?> AddAsync(
        long userIdentifier,
        IModifyDeviceModel modifyDeviceModel,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(
        long deviceIdentifier,
        IModifyDeviceModel modifyDeviceModel,
        CancellationToken cancellationToken = default);
        
    Task<bool> DeleteAsync(long identifier, CancellationToken cancellationToken = default);
}