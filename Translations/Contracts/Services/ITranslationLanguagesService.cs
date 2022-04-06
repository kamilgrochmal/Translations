using Translations.Models.Admin.Languages;
using Translations.Models.Pagination;

namespace Translations.Contracts.Services;

public interface ITranslationLanguagesService
{
    Task<PagedResponse<TranslationLanguageDto>> BrowseAsync(PaginationFilter paginationFilter = null);
    Task<TranslationLanguageDto> GetAsync(int translationLanguageId);
    Task<int> CreateAsync(CreateTranslationLanguageDto dto);
    Task<int> UpdateAsync(CreateTranslationLanguageDto dto, int translationLanguageId);
    Task DeleteAsync(int translationLanguageId);
    

}