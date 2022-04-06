using Domain.Translations;
using Translations.Contracts.Repositories;

namespace Translations.Repositories;

public class TranslationRowsRepository : ITranslationRowsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TranslationRowsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<TranslationRow> GetAsync(int translationRowId)
    {
        return await _dbContext.TranslationRows.FirstOrDefaultAsync(a => a.Id == translationRowId);
    }

    public async Task CreateAsync(TranslationRow translationRow)
    {
        await _dbContext.TranslationRows.AddAsync(translationRow);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TranslationRow translationRow)
    {
        _dbContext.TranslationRows.Update(translationRow);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TranslationRow translationRow)
    {
        _dbContext.TranslationRows.Remove(translationRow);
        await _dbContext.SaveChangesAsync();
    }
} 