using Translations.Contracts.Repositories;
using Translations.Contracts.Services;
using Translations.Exceptions;
using Translations.Helpers;
using Translations.Models.Admin.Languages;
using Translations.Models.Pagination;

namespace Translations.Services;

public class TranslationLanguagesService : ITranslationLanguagesService
{
    private readonly ITranslationLanguagesRepository _translationLanguagesRepository;
    private readonly ApplicationDbContext _dbContext;


    public TranslationLanguagesService(
        ITranslationLanguagesRepository translationLanguagesRepository, ApplicationDbContext dbContext)
    {
        _translationLanguagesRepository = translationLanguagesRepository;
        _dbContext = dbContext;
    }

    public async Task<PagedResponse<TranslationLanguageDto>> BrowseAsync(PaginationFilter paginationFilter = null)
    {
        var query = _dbContext.TranslationLanguages.AsNoTracking().Select(a => new TranslationLanguageDto()
        {
            Id = a.Id,
            Code = a.Code,
            Name = a.Name,
            NameENG = a.NameENG
        });
        var pagedResponse = await PaginationHelper.GetFilteredPagedResponse(query, paginationFilter);


        return pagedResponse;
    }


    public async Task<TranslationLanguageDto> GetAsync(int translationLanguageId)
    {
        var translationLanguage = await _dbContext.TranslationLanguages
              .AsNoTracking()
              .Where(a => a.Id == translationLanguageId)
              .Select(tr => new TranslationLanguageDto()
              {
                  Id = tr.Id,
                  Code = tr.Code,
                  Name = tr.Name,
                  NameENG = tr.NameENG
              }).FirstOrDefaultAsync() ?? throw new NotFoundException($"Translation language with requested id: '{translationLanguageId}' could not be found.");

        return translationLanguage;
    }

    public async Task<int> CreateAsync(CreateTranslationLanguageDto dto)
    {
        var translationLanguage = new TranslationLanguage()
        {
            Code = dto.Code,
            Name = dto.Name,
            NameENG = dto.NameENG
        };

        await _translationLanguagesRepository.CreateAsync(translationLanguage);
        return translationLanguage.Id;
    }

    public async Task<int> UpdateAsync(CreateTranslationLanguageDto dto, int translationLanguageId)
    {
        var translationLanguage = await _translationLanguagesRepository.GetAsync(translationLanguageId) ??
                                  throw new NotFoundException(
                                      $"Translation language with requested id: '{translationLanguageId}' could not be found.");

        translationLanguage.Code = dto.Code;
        translationLanguage.Name = dto.Name;
        translationLanguage.NameENG = dto.NameENG;

        await _translationLanguagesRepository.UpdateAsync(translationLanguage);

        return translationLanguage.Id;
    }

    public async Task DeleteAsync(int translationLanguageId)
    {
        var translationLanguage = await _translationLanguagesRepository.GetAsync(translationLanguageId) ??
                                  throw new NotFoundException(
                                      $"Translation language with requested id: '{translationLanguageId}' could not be found.");
        await _translationLanguagesRepository.DeleteAsync(translationLanguage);
    }
}