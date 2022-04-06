using Domain.Translations;

namespace Translations.Contracts.Repositories;

public interface ITranslationLanguagesRepository
{
    Task<TranslationLanguage> GetAsync(int translationLanguageId);
    Task CreateAsync(TranslationLanguage translationLanguage);
    Task UpdateAsync(TranslationLanguage translationLanguage);
    Task DeleteAsync(TranslationLanguage translationLanguage);
}