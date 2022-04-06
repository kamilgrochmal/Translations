using Microsoft.EntityFrameworkCore;
using Translations.Helpers.PaginationExtensions;
using Translations.Models.Pagination;

namespace Translations.Helpers;

public static class PaginationHelper
{
    public static async Task<PagedResponse<T>> GetFilteredPagedResponse<T>(IQueryable<T> queries,
        PaginationFilter paginationFilter) where T : class
    {
        paginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize,
            paginationFilter.SortField, paginationFilter.SortDir, paginationFilter.Filters);
        var filtersQuery = queries.ToFilterView(paginationFilter);
        var pageData = GetPaginationData(filtersQuery, paginationFilter);
        filtersQuery = filtersQuery.ToPaginationView(paginationFilter);
        var result = await filtersQuery.ToListAsync();
        var pagedResponse = new PagedResponse<T>(result);
        pagedResponse.PagedData = pageData;
        return pagedResponse;
    }

    public static async Task<PagedResponse<T>> GetFilteredPagedResponseWithoutSort<T>(IQueryable<T> queries,
        PaginationFilter paginationFilter) where T : class
    {
        paginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize,
            paginationFilter.SortField, paginationFilter.SortDir, paginationFilter.Filters);
        var filtersQuery = queries;
        var pageData = GetPaginationData(filtersQuery, paginationFilter);
        filtersQuery = filtersQuery.ToPaginationView(paginationFilter);
        var result = await filtersQuery.ToListAsync();
        var pagedResponse = new PagedResponse<T>(result);
        pagedResponse.PagedData = pageData;
        return pagedResponse;
    }


    private static PagedData GetPaginationData<T>(IQueryable<T> queries, PaginationFilter paginationFilter)
        where T : class
    {
        PagedData pagedData = new PagedData();
        pagedData.PageNumber = paginationFilter.PageNumber;
        pagedData.PageSize = paginationFilter.PageSize;

        //Get Result Count
        pagedData.ResultsCount = queries.Count();
        double pages = (double) pagedData.ResultsCount / (double) pagedData.PageSize;
        if (double.IsInfinity(pages))
        {
            pagedData.Pages = 0;
        }
        else
        {
            pagedData.Pages = (int) pages;
            if ((pages - (int) pages) != 0)
            {
                pagedData.Pages += 1;
            }
        }

        return pagedData;
    }
}