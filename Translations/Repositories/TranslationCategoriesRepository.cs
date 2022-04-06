using Domain.Translations;
using Translations.Contracts.Repositories;

namespace Translations.Repositories;

public class TranslationCategoriesRepository : ITranslationCategoriesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TranslationCategoriesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<TranslationCategory> GetAsync(int translationCategoryId)
    {
       return await _dbContext.TranslationCategories.FirstOrDefaultAsync(a => a.Id == translationCategoryId);
    }

    public async Task CreateAsync(TranslationCategory translationCategory)
    {
        await _dbContext.TranslationCategories.AddAsync(translationCategory);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TranslationCategory translationCategory)
    {
        _dbContext.TranslationCategories.Update(translationCategory);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TranslationCategory translationCategory)
    {
        _dbContext.TranslationCategories.Remove(translationCategory);
        await _dbContext.SaveChangesAsync();
    }
}
