using DeviceRepository.Common.Page;
using Microsoft.EntityFrameworkCore;

namespace DeviceRepository.Repositories;

public static class DeviceRepositoryHelper
{
    public static async Task<PagedResult<T>> GetPaged<T>(
        this IQueryable<T> query,
        PageInfo pageInfo) 
        where T : class
    {
        var rowCount = query.Count();
        var pageCount = (int)Math.Ceiling((double)rowCount / pageInfo.Size);
        var skip = (pageInfo.Page - 1) * pageInfo.Size;
        var result = await query
            .Skip(skip)
            .Take(pageInfo.Size)
            .ToArrayAsync()
            .ConfigureAwait(false);
        
        return new PagedResult<T>(result)
        {
            CurrentPage = pageInfo.Page,
            PageSize = pageInfo.Size,
            RowCount = rowCount,
            PageCount = pageCount
        };
    }
}