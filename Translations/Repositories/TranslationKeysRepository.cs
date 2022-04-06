using Domain.Translations;
using Translations.Contracts.Repositories;

namespace Translations.Repositories;

public class TranslationKeysRepository : ITranslationKeysRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TranslationKeysRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<TranslationKey> GetAsync(int translationKeyId)
    {
        return await _dbContext.TranslationKeys.Include(q => q.TranslationCategory)
            .Include(q => q.Translations).ThenInclude(q => q.TranslationLanguage).FirstOrDefaultAsync(a => a.Id == translationKeyId);
    }

    public async Task CreateAsync(TranslationKey translationKey)
    {
        await _dbContext.TranslationKeys.AddAsync(translationKey);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TranslationKey translationKey)
    {
        _dbContext.TranslationKeys.Update(translationKey);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TranslationKey translationKey)
    {
        _dbContext.TranslationKeys.Remove(translationKey);
        await _dbContext.SaveChangesAsync();
    }
}
   