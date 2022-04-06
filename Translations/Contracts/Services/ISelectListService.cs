using Translations.Models.Admin;

namespace Translations.Contracts.Services;

public interface ISelectListService
{
    Task<List<ListItemDto>> GetTranslationLanguagesList();
    Task<List<ListItemDto>> GetTranslationCategoriesList();
    Task<List<ListItemDto>> GetTranslationKeysList();
}