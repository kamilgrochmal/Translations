using Translations.Models.Admin.Rows;
using Translations.Models.Pagination;

namespace Translations.Contracts.Services;

public interface ITranslationRowsService
{
    Task<PagedResponse<TranslationRowDto>> BrowseAsync(PaginationFilter paginationFilter);
    Task<int> CreateAsync(CreateTranslationRowDto createTranslationRowDto);
    Task<TranslationRowDto> GetAsync(int translationRowId);
    Task<TranslationRowDto> GetAsync(int translationKeyId, int translationLanguageId);
    Task<int> UpdateAsync(CreateTranslationRowDto dto, int translationRowId);
    Task DeleteAsync(int translationRowId);


}