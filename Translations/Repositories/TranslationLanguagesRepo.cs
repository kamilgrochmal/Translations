using Domain.Translations;
using Translations.Contracts.Repositories;

namespace Translations.Repositories;

public class TranslationLanguagesRepository : ITranslationLanguagesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TranslationLanguagesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<TranslationLanguage> GetAsync(int translationLanguageId)
    {
        return await _dbContext.TranslationLanguages.FirstOrDefaultAsync(a => a.Id == translationLanguageId);
    }

    public async Task CreateAsync(TranslationLanguage translationLanguage)
    {
        await _dbContext.TranslationLanguages.AddAsync(translationLanguage);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TranslationLanguage translationLanguage)
    {
        _dbContext.TranslationLanguages.Update(translationLanguage);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TranslationLanguage translationLanguage)
    {
        _dbContext.TranslationLanguages.Remove(translationLanguage);
        await _dbContext.SaveChangesAsync();
    }
}