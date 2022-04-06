using Domain.Translations;

namespace Translations.Contracts.Repositories;

public interface ITranslationRowsRepository
{
    Task<TranslationRow> GetAsync(int translationRowId);
    Task CreateAsync(TranslationRow translationRow);
    Task UpdateAsync(TranslationRow translationRow);
    Task DeleteAsync(TranslationRow translationRow);
}