using Translations.Contracts.Repositories;
using Translations.Contracts.Services;
using Translations.Exceptions;
using Translations.Helpers;
using Translations.Models.Admin.Keys;
using Translations.Models.Pagination;

namespace Translations.Services;

public class TranslationKeysService : ITranslationKeysService
{
    private readonly ITranslationKeysRepository _translationKeysRepository;
    private readonly ApplicationDbContext _dbContext;


    public TranslationKeysService(
        ITranslationKeysRepository translationKeysRepository,
        ApplicationDbContext dbContext)
    {
        _translationKeysRepository = translationKeysRepository;
        _dbContext = dbContext;
    }

    public async Task<PagedResponse<TranslationKeyDto>> BrowseAsync(PaginationFilter paginationFilter)
    {
        var query = _dbContext.TranslationKeys
            .Include(q => q.TranslationCategory)
            .Include(q => q.Translations).ThenInclude(q => q.TranslationLanguage)
            .AsNoTracking();
        query = SetFilters(query, paginationFilter);

        var queryHelper = query.Select(src => new TranslationKeyDto()
        {
            Id = src.Id,
            Code = src.Code,
            Description = src.Description,
            CategoryId = src.TranslationCategoryId,
            CategoryName = src.TranslationCategory.Name,
        }).AsQueryable();


        var pagedResponse = await PaginationHelper.GetFilteredPagedResponse(queryHelper, paginationFilter);


        foreach (var q in pagedResponse.Data)
        {
            q.TranslationsInLanguages = await _dbContext.TranslationRows.Where(a => a.TranslationKeyId == q.Id)
                .Select(a => a.TranslationLanguage.Name).Distinct().ToListAsync();
        }


        return pagedResponse;
    }


    public async Task<TranslationKeyDto> GetAsync(int translationKeyId)
    {
        var translationKey = await _dbContext.TranslationKeys
                                 .Include(q => q.TranslationCategory)
                                 .Include(q => q.Translations)
                                 .ThenInclude(q => q.TranslationLanguage)
                                 .Select(src => new TranslationKeyDto()
                                 {
                                     Id = src.Id,
                                     Code = src.Code,
                                     Description = src.Description,
                                     CategoryId = src.TranslationCategoryId,
                                     CategoryName = src.TranslationCategory.Name,
                                     IsBackend = src.IsBackend,
                                     IsFrontend = src.IsFrontend
                                 }).FirstOrDefaultAsync(tk => tk.Id == translationKeyId) ??
                             throw new NotFoundException(
                                 $"Translation key with requested id {translationKeyId} could not be found.");


        // Pretty spaghetti code but for now it works
        var languagesWithTranslations = await _dbContext.TranslationLanguages.Select(a =>
            new TranslationLanguagesWithTranslationsDto()
            {
                TranslationLanguageId = a.Id,
                TranslationLanguageName = a.Name,
            }).ToListAsync();

        foreach (var language in languagesWithTranslations)
        {
            language.TranslationValue = await _dbContext.TranslationRows
                .Where(a => a.TranslationLanguageId == language.TranslationLanguageId &&
                            a.TranslationKeyId == translationKeyId).Select(c => c.Value).FirstOrDefaultAsync();

            language.TranslationValueId = await _dbContext.TranslationRows
                .Where(a => a.TranslationLanguageId == language.TranslationLanguageId &&
                            a.TranslationKeyId == translationKeyId).Select(c => c.Id).FirstOrDefaultAsync();
        }

        translationKey.LanguagesWithTranslations = languagesWithTranslations;

        return translationKey;
    }

    public async Task<int> CreateAsync(CreateTranslationKeyDto dto)
    {
        var translationKey = new TranslationKey()
        {
            Code = dto.Code,
            Description = dto.Description,
            IsBackend = dto.IsBackend,
            IsFrontend = dto.IsFrontend,
            TranslationCategoryId = dto.TranslationCategoryId
        };
        bool translationKeyAlreadyExists = await _dbContext.TranslationKeys.Where(q => q.Code == translationKey.Code)
            .AnyAsync(q => q.TranslationCategoryId == translationKey.TranslationCategoryId);

        if (translationKeyAlreadyExists)
            throw new TranslationKeyAlreadyExistsException(translationKey.Code);

        await _translationKeysRepository.CreateAsync(translationKey);
        return translationKey.Id;
    }

    public async Task<int> UpdateAsync(CreateTranslationKeyDto dto, int translationKeyId)
    {
        var translationKey = await _translationKeysRepository.GetAsync(translationKeyId) ??
                             throw new NotFoundException(
                                 $"Translation key with requested id {translationKeyId} could not be found.");

        translationKey.Code = dto.Code;
        translationKey.Description = dto.Description;
        translationKey.IsBackend = dto.IsBackend;
        translationKey.IsFrontend = dto.IsFrontend;
        translationKey.TranslationCategoryId = dto.TranslationCategoryId;

        await _translationKeysRepository.UpdateAsync(translationKey);
        return translationKey.Id;
    }

    public async Task DeleteAsync(int translationKeyId)
    {
        var translationKey = await _translationKeysRepository.GetAsync(translationKeyId) ??
                             throw new NotFoundException(
                                 $"Translation key with requested id {translationKeyId} could not be found.");
        await _translationKeysRepository.DeleteAsync(translationKey);
    }

    public async Task<GetAnotherEmptyKeyDto> GetAnotherEmptyKey(int currentLanguageId, int currentKeyId)
    {
        var anotherKeys = _dbContext.TranslationKeys.AsNoTracking()
            .Where(a => a.Id != currentKeyId && !_dbContext.TranslationRows.Any(c =>
                c.TranslationKeyId == a.Id && c.TranslationLanguageId == currentLanguageId));

        var currentCategoryId = await _dbContext.TranslationRows.Include(c => c.TranslationKey)
            .Where(a => a.TranslationLanguageId == currentLanguageId && a.TranslationKeyId == currentKeyId)
            .Select(a => a.TranslationKey.TranslationCategory.Id).FirstOrDefaultAsync();
        
        var helperQuery = anotherKeys.Where(a => a.TranslationCategoryId == currentCategoryId);

        GetAnotherEmptyKeyDto key;
        
        if (helperQuery.Any())
        {
            key = await helperQuery.Select(a => new GetAnotherEmptyKeyDto()
            {
                TranslationKey = a.Code,
                TranslationKeyId = a.Id
            }).FirstOrDefaultAsync();

            if(key is not null)
                key.TranslationLanguage = await _dbContext.TranslationLanguages.Where(a => a.Id == currentKeyId).Select(a => a.Name).FirstOrDefaultAsync();

            return key;
        }
        
        key = await anotherKeys.Select(a => new GetAnotherEmptyKeyDto()
        {
            TranslationKey = a.Code,
            TranslationKeyId = a.Id
        }).FirstOrDefaultAsync();

        if(key is not null)
            key.TranslationLanguage = await _dbContext.TranslationLanguages.Where(a => a.Id == currentKeyId).Select(a => a.Name).FirstOrDefaultAsync();

        return key;
   

           
        
    }
        
    


    private IQueryable<TranslationKey> SetFilters(IQueryable<TranslationKey> query, PaginationFilter paginationFilter)
    {
        var name = paginationFilter.Filters.FirstOrDefault(q => q.Field == "name");
        if (name is not null)
        {
            var searchTxt = name.Value;
            query = query.Where(a => a.Code.Contains(searchTxt) || a.Description.Contains(searchTxt));
        }

        var category = paginationFilter.Filters.FirstOrDefault(q => q.Field == "category");
        if (category is not null)
        {
            query = query.Where(a => a.TranslationCategoryId.ToString() == category.Value);
        }

        var lackOfTranslationInLanguage =
            paginationFilter.Filters.FirstOrDefault(q => q.Field == "lackOfTranslationsInLanguage");
        if (lackOfTranslationInLanguage is not null)
        {
            var languageId = Convert.ToInt32(lackOfTranslationInLanguage.Value);
            query = query.Where(a => a.Translations.All(c => c.TranslationLanguageId != languageId));
        }

        return query;
    }
}