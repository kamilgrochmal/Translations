using Translations.Contracts.Repositories;
using Translations.Contracts.Services;
using Translations.Exceptions;
using Translations.Helpers;
using Translations.Models.Admin.Categories;
using Translations.Models.Pagination;

namespace Translations.Services;

public class TranslationCategoriesService : ITranslationCategoriesService
{
    private readonly ITranslationCategoriesRepository _translationCategoriesRepository;
    private readonly ApplicationDbContext _dbContext;


    public TranslationCategoriesService(
        ITranslationCategoriesRepository translationCategoriesRepository, ApplicationDbContext dbContext)
    {
        _translationCategoriesRepository = translationCategoriesRepository;
        _dbContext = dbContext;
    }

    public async Task<PagedResponse<TranslationCategoryDto>> BrowseAsync(PaginationFilter paginationFilter = null)
    {
        var query = _dbContext.TranslationCategories.AsNoTracking();
        query = SetFilters(query, paginationFilter);
        
        var queryHelper = query.Select(a => new TranslationCategoryDto()
        {
            Id = a.Id,
            Name = a.Name,
            Description = a.Description
        });
        var pagedResponse = await PaginationHelper.GetFilteredPagedResponse(queryHelper, paginationFilter);

        return pagedResponse;
    }

    public async Task<TranslationCategoryDto> GetAsync(long translationCategoryId)
    {
        var translationCategory = await _dbContext.TranslationCategories.AsNoTracking()
              .Where(a => a.Id == translationCategoryId)
              .Include(q => q.TranslationKeys)
              .ThenInclude(q => q.Translations)
              .ThenInclude(q => q.TranslationLanguage)
              .Select(tc => new TranslationCategoryDto()
              {
                  Id = tc.Id,
                  Name = tc.Name,
                  Description = tc.Description
              }).FirstOrDefaultAsync() ?? throw new NotFoundException($"Translation category with requested id: '{translationCategoryId}' could not be found");

        var translations = await _dbContext.TranslationKeys.Where(a => a.TranslationCategoryId == translationCategoryId).Select(a =>
            new TranslationKeysWithLanguagesDto()
            {
                TranslationKeyId = a.Id,
                TranslationKey = a.Code,
                TranslationCategoryId = a.TranslationCategoryId
            }).ToListAsync();

        foreach (var tc in translations)
        {
            tc.TranslatedLanguages = await _dbContext.TranslationRows
                .Where(x => x.TranslationKeyId == tc.TranslationKeyId).Select(a => new LanguageDto()
                {
                    Id = a.TranslationLanguageId,
                    Code = a.TranslationLanguage.Code,
                    Name = a.TranslationLanguage.Name
                }).ToListAsync();
        }

        translationCategory.TranslationKeys = translations;

        return translationCategory;
    }

    public async Task<int> CreateAsync(CreateTranslationCategoryDto dto)
    {
        bool categoryAlreadyExists = await _dbContext.TranslationCategories.AnyAsync(q => q.Name == dto.Name);
        if (categoryAlreadyExists)
            throw new TranslationCategoryAlreadyExistsException();

        var translationCategory = new TranslationCategory()
        {
            Name = dto.Name,
            Description = dto.Description
        };

        await _translationCategoriesRepository.CreateAsync(translationCategory);
        return translationCategory.Id;
    }

    public async Task<int> UpdateAsync(CreateTranslationCategoryDto dto, int translationCategoryId)
    {
        var translationCategory = await _translationCategoriesRepository.GetAsync(translationCategoryId) ?? 
                                  throw new NotFoundException($"Translation category with requested id {translationCategoryId} could not be found.");

        translationCategory.Name = dto.Name;
        translationCategory.Description = dto.Description;
        await _translationCategoriesRepository.UpdateAsync(translationCategory);

        return translationCategory.Id;
    }

    public async Task DeleteAsync(int translationCategoryId)
    {
        var translationCategory = await _translationCategoriesRepository.GetAsync(translationCategoryId) ?? 
                                  throw new NotFoundException($"Translation category with requested id {translationCategoryId} could not be found.");
        await _translationCategoriesRepository.DeleteAsync(translationCategory);
    }

    private IQueryable<TranslationCategory> SetFilters(IQueryable<TranslationCategory> query,
        PaginationFilter paginationFilter)
    {
        var name = paginationFilter.Filters.FirstOrDefault(q => q.Field.ToLower() == "name");
        if (name is null) return query;
        var searchTxt = name.Value;
        query = query.Where(a => a.Name.Contains(searchTxt));

        return query;
    }
}