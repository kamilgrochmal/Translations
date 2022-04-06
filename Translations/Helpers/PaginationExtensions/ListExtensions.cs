using Microsoft.EntityFrameworkCore;
using Translations.Models.Pagination;

namespace Translations.Helpers.PaginationExtensions;

public static class ListExtensions
{
    public static async Task<List<T>> ToPaginatedList<T>(this IQueryable<T> queries, PaginationFilter paginationFilter = null)
    {
        if (paginationFilter != null)
        {
            int skipRecords = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queries.Skip(skipRecords)
                .Take(paginationFilter.PageSize)
                .ToListAsync();
        }
        return await queries.ToListAsync();
    }
}