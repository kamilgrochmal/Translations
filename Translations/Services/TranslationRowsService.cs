using Translations.Contracts.Repositories;
using Translations.Contracts.Services;
using Translations.Exceptions;
using Translations.Helpers;
using Translations.Models.Admin.Rows;
using Translations.Models.Pagination;

namespace Translations.Services;

public class TranslationRowsService : ITranslationRowsService
{
    private readonly ITranslationRowsRepository _translationRowsRepository;
    private readonly ApplicationDbContext _db;

    public TranslationRowsService(
        ITranslationRowsRepository translationRowsRepository, ApplicationDbContext db)
    {
        _translationRowsRepository = translationRowsRepository;
        _db = db;
    }

    public async Task<PagedResponse<TranslationRowDto>> BrowseAsync(PaginationFilter paginationFilter)
    {
        var query = _db.TranslationRows.AsNoTracking();
        query = SetFilters(query, paginationFilter);
        var queryHelper = query.Include(q => q.TranslationLanguage)
            .Include(q => q.TranslationKey)
            .ThenInclude(q => q.TranslationCategory)
            .Select(a => new TranslationRowDto()
            {
                Id = a.Id,
                Value = a.Value,
                TranslationCategory = a.TranslationKey.TranslationCategory.Name,
                TranslationCategoryId = a.TranslationKey.TranslationCategory.Id,
                TranslationKey = a.TranslationKey.Code,
                TranslationKeyId = a.TranslationKey.Id,
                TranslationLanguage = a.TranslationLanguage.Name,
                TranslationLanguageId = a.TranslationLanguage.Id
            }).AsQueryable();

        var pagedResponse = await PaginationHelper.GetFilteredPagedResponse(queryHelper, paginationFilter);
        return pagedResponse;
    }


    public async Task<TranslationRowDto> GetAsync(int translationRowId)
    {
        var polishId =await _db.TranslationLanguages.Where(a => a.Code == "pl").Select(a => a.Id).FirstOrDefaultAsync();
        var query = await _db.TranslationRows
            .Include(q => q.TranslationLanguage)
            .Include(q => q.TranslationKey).ThenInclude(q => q.TranslationCategory)
            .Select(a => new TranslationRowDto()
            {
                Id = a.Id,
                Value = a.Value,
                TranslationCategory = a.TranslationKey.TranslationCategory.Name,
                TranslationCategoryId = a.TranslationKey.TranslationCategory.Id,
                TranslationKey = a.TranslationKey.Code,
                TranslationKeyId = a.TranslationKey.Id,
                TranslationLanguage = a.TranslationLanguage.Name,
                TranslationLanguageId = a.TranslationLanguage.Id,
                KeyTranslationDescription = a.TranslationKey.Description
            }).FirstOrDefaultAsync(a => a.Id == translationRowId);

        if (query is not null)
            query.PolishTranslationRow = await _db.TranslationRows
                .Where(a => a.TranslationKeyId == query.TranslationKeyId && a.TranslationLanguageId == polishId)
                .Select(a => a.Value).FirstOrDefaultAsync();
        
        return query;
    }
    public async Task<TranslationRowDto> GetAsync(int translationKeyId, int translationLanguageId)
    {
        var dto = await _db.TranslationKeys.Where(a => a.Id == translationKeyId).Select(tk =>
            new TranslationRowDto()
            {
                TranslationKeyId = tk.Id,
                TranslationKey = tk.Code,
                TranslationCategory = tk.TranslationCategory.Name,
                KeyTranslationDescription = tk.Description
            }).FirstOrDefaultAsync();

        var polishId = await _db.TranslationLanguages.Where(a => a.Code == "pl").Select(a => a.Id).FirstOrDefaultAsync();
        if (dto is not null)
        {
            dto.TranslationLanguageId = translationLanguageId;
            dto.TranslationLanguage = await _db.TranslationLanguages.Where(a => a.Id == translationLanguageId).Select(a => a.Name).FirstOrDefaultAsync();
            dto.PolishTranslationRow = await _db.TranslationRows
                .Where(a => a.TranslationKeyId == translationKeyId && a.TranslationLanguageId == polishId)
                .Select(a => a.Value).FirstOrDefaultAsync();
        }

        return dto;
    }

    public async Task<int> CreateAsync(CreateTranslationRowDto dto)
    {
        var translationRow = new TranslationRow()
        {
            Value = dto.Value,
            TranslationKeyId = dto.TranslationKeyId,
            TranslationLanguageId = dto.TranslationLanguageId
        };

        var translationAlreadyExists = await _db.TranslationRows
            .Where(q => q.TranslationKeyId == translationRow.TranslationKeyId)
            .AnyAsync(q => q.TranslationLanguageId == translationRow.TranslationLanguageId);

        if (translationAlreadyExists)
            throw new TranslationInThisLanguageAlreadyExistsException();

        await _translationRowsRepository.CreateAsync(translationRow);

        return translationRow.Id;
    }

    public async Task<int> UpdateAsync(CreateTranslationRowDto dto, int translationRowId)
    {
        var translationRow = await _translationRowsRepository.GetAsync(translationRowId) ??
                             throw new NotFoundException(
                                 $"Translation with requested id: '{translationRowId}' could not be found");


        translationRow.Value = dto.Value;
        translationRow.TranslationKeyId = dto.TranslationKeyId;
        translationRow.TranslationLanguageId = dto.TranslationLanguageId;

        await _translationRowsRepository.UpdateAsync(translationRow);

        return translationRow.Id;
    }

    public async Task DeleteAsync(int translationRowId)
    {
        var translationRow = await _translationRowsRepository.GetAsync(translationRowId) ??
                             throw new NotFoundException(
                                 $"Translation with requested id: '{translationRowId}' could not be found");
        ;
        await _translationRowsRepository.DeleteAsync(translationRow);
    }


    private IQueryable<TranslationRow> SetFilters(IQueryable<TranslationRow> query, PaginationFilter paginationFilter)
    {
        var name = paginationFilter.Filters.FirstOrDefault(q => q.Field.ToLower() == "name");
        if (name != null)
        {
            var searchTxt = name.Value;
            query = query.Where(a => a.Value.Contains(searchTxt) || a.TranslationKey.Code.Contains(searchTxt));
        }

        var language = paginationFilter.Filters.FirstOrDefault(q => q.Field.ToLower() == "language");
        if (language != null)
        {
            query = query.Where(a => a.TranslationLanguageId.ToString() == language.Value);
        }

        var category = paginationFilter.Filters.FirstOrDefault(q => q.Field.ToLower() == "category");
        if (category != null)
        {
            query = query.Where(a => a.TranslationKey.TranslationCategoryId.ToString() == category.Value);
        }

        return query;
    }
}