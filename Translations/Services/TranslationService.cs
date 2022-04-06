using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using Translations.Configurations;
using Translations.Contracts.Services;
using Translations.Exceptions;
using Translations.Models.Admin.Languages;

namespace Translations.Services;

public class TranslationService : ITranslationService
{
    private readonly ApplicationDbContext _db;
    private readonly IMemoryCache _memoryCache;


    public TranslationService(ApplicationDbContext db, IMemoryCache memoryCache)
    {
        _db = db;
        _memoryCache = memoryCache;
    }

    
    public async Task<IEnumerable<TranslationLanguageDto>> GetTranslationLanguagesList()
    {
        var translations = await _db.TranslationLanguages.AsNoTracking().Select(a => new TranslationLanguageDto()
        {
            Id = a.Id,
            Name = a.Name,
            NameENG = a.NameENG,
            Code = a.Code
        }).ToListAsync();

        return translations;
    }

    public async Task<JObject> GetAllTranslations()
    {
        var translationCategories = await _db.TranslationCategories
            .Include(q => q.TranslationKeys).ThenInclude(q => q.Translations)
            .ToListAsync();
        var translationLanguages = await _db.TranslationLanguages.ToListAsync();

        dynamic json = new JObject();
        json.resources = new JObject();
        foreach (var translationLanguage in translationLanguages)
        {
            json.resources[translationLanguage.Code] = new JObject();
            foreach (var translationCategory in translationCategories)
            {
                json.resources[translationLanguage.Code][translationCategory.Name] = new JObject();
                foreach (var translationKey in translationCategory.TranslationKeys.Where(q => q.IsFrontend))
                {
                    foreach (var translation in translationKey.Translations.Where(q =>
                                 q.TranslationLanguageId == translationLanguage.Id))
                    {
                        json.resources[translationLanguage.Code][translationCategory.Name][translationKey.Code] =
                            translation.Value;
                    }
                }
            }
        }
        return json;
    }

    public async Task<JObject> GetTranslationByCode(string code)
    {
        var translationsKey = $"translations_{code}";

        var output = _memoryCache.Get<JObject>(translationsKey);

        if (output is null)
        {
            var translationLanguage = await _db.TranslationLanguages.FirstOrDefaultAsync(q => q.Code == code) ??
                                      throw new NotFoundException($"Translation language {code} could not be found.");

            var queryHelper = _db.TranslationRows.Include(a=>a.TranslationKey).ThenInclude(a => a.TranslationCategory).Where(a =>
                a.TranslationLanguageId == translationLanguage.Id && a.TranslationKey.IsFrontend == true);

            var categories = await queryHelper.Select(a => a.TranslationKey.TranslationCategory).ToListAsync();

            output = new JObject();

            foreach (var translationCategory in categories)
            {
                output[translationCategory.Name] = new JObject();
                foreach (var translationRow in queryHelper.Where(a =>
                             a.TranslationKey.TranslationCategory.Id == translationCategory.Id))
                {
                    output[translationCategory.Name][translationRow.TranslationKey.Code] = translationRow.Value;
                }
            }
            _memoryCache.Set(translationsKey, output, TimeSpan.FromMinutes(TranslationConfig.CachePersistenceTime));

        }
        return output;
    }
}