using Domain.Translations;

namespace Translations.Contracts.Repositories;

public interface ITranslationCategoriesRepository
{
    Task<TranslationCategory> GetAsync(int translationCategoryId);
    Task CreateAsync(TranslationCategory translationCategory);
    Task UpdateAsync(TranslationCategory translationCategory);
    Task DeleteAsync(TranslationCategory translationCategory);
}