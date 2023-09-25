using DeviceRepository.Common.Page;

namespace DeviceRepository.Repositories;

public static class DeviceRepositoryHelper
{
    public static PagedResult<T> GetPaged<T>(
        this IQueryable<T> query,
        PageInfo pageInfo) where T : class
    {
        var rowCount = query.Count();
        var pageCount = (int)Math.Ceiling((double) rowCount / pageInfo.Size);
        var skip = (pageInfo.Page - 1) * pageInfo.Size;
        var result = query.Skip(skip).Take(pageInfo.Size);

        return new PagedResult<T>(result)
        {
            CurrentPage = pageInfo.Page,
            PageSize = pageInfo.Size,
            RowCount = rowCount,
            PageCount = pageCount
        };
    }
}