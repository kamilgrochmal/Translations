using System.Globalization;
using Translations.Contracts.Services;

namespace Translations.Services;

public class ViewTranslationService : IViewTranslationService
{
    private readonly ApplicationDbContext _db;
    private readonly ICacheService _cacheService;

    public ViewTranslationService(ApplicationDbContext db, ICacheService cacheService)
    {
        _db = db;
        _cacheService = cacheService;
    }

    public async Task<string> Get(string key, string category)
    {
        key = key.Trim().ToLower();
        category = category.Trim().ToLower();
        
        var currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

        var categoryId = await _db.TranslationCategories.AsNoTracking().Where(a=> a.Name.ToLower() == category).Select(a => a.Id).FirstOrDefaultAsync();
        var keyId = await _db.TranslationKeys.AsNoTracking().Where(a=> a.Code.ToLower() == key).Select(a => a.Id).FirstOrDefaultAsync();
        
        var translation = await _db.TranslationRows.Include(a=>a.TranslationKey).ThenInclude(a => a.TranslationCategory)
            .Where(a => a.TranslationKey.TranslationCategoryId == categoryId && a.TranslationKeyId == keyId && a.TranslationLanguage.Code.ToLower() == currentCulture)
            .AsNoTracking()
            .Select(a => a.Value)
            .FirstOrDefaultAsync() ?? "$gurururururur$";

        return translation;
    }
}