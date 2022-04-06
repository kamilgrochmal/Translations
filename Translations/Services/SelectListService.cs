using Translations.Contracts.Services;
using Translations.Models.Admin;

namespace Translations.Services;

public class SelectListService : ISelectListService
{
    private readonly ApplicationDbContext _dbContext;

    public SelectListService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ListItemDto>> GetTranslationLanguagesList()
        => await _dbContext.TranslationLanguages.OrderByDescending(tl => tl.Id).Select(src => new ListItemDto()
        {
            Value = src.Id.ToString(),
            Text = src.Name
        }).ToListAsync();


    public async Task<List<ListItemDto>> GetTranslationCategoriesList()
        => await _dbContext.TranslationCategories.OrderByDescending(tk => tk.Id).Select(src => new ListItemDto()
        {
            Value = src.Id.ToString(),
            Text = src.Name
        }).ToListAsync();

    public async Task<List<ListItemDto>> GetTranslationKeysList()
        => await _dbContext.TranslationKeys.OrderByDescending(tk => tk.Id).Select(src => new ListItemDto()
        {
            Value = src.Id.ToString(),
            Text = src.Code
        }).ToListAsync();
    
}