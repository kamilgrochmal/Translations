using Domain.Translations;

namespace Translations.Contracts.Repositories;

public interface ITranslationKeysRepository
{
    Task<TranslationKey> GetAsync(int translationKeyId);
    Task CreateAsync(TranslationKey translationKey);
    Task UpdateAsync(TranslationKey translationKey);
    Task DeleteAsync(TranslationKey translationKey);
}