using Translations.Models.Admin.Keys;
using Translations.Models.Pagination;

namespace Translations.Contracts.Services;

public interface ITranslationKeysService
{
    Task<PagedResponse<TranslationKeyDto>> BrowseAsync(PaginationFilter paginationFilter);
    Task<int> CreateAsync(CreateTranslationKeyDto createTranslationKeyDto);
    Task<TranslationKeyDto> GetAsync(int translationKeyId);
    Task<int> UpdateAsync(CreateTranslationKeyDto dto, int translationKeyId);
    Task DeleteAsync(int translationKeyId);
    Task<GetAnotherEmptyKeyDto> GetAnotherEmptyKey(int currentLanguageId, int currentKeyId);


}
