using Translations.Models.Admin.Categories;
using Translations.Models.Pagination;

namespace Translations.Contracts.Services;

public interface ITranslationCategoriesService
{
    Task<PagedResponse<TranslationCategoryDto>> BrowseAsync(PaginationFilter paginationFilter = null);
    Task<TranslationCategoryDto> GetAsync(long translationCategoryId);
    Task<int> CreateAsync(CreateTranslationCategoryDto dto);
    Task<int> UpdateAsync(CreateTranslationCategoryDto dto, int translationCategoryId);
    Task DeleteAsync(int translationCategoryId);
}