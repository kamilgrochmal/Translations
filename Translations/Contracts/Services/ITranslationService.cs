using Newtonsoft.Json.Linq;
using Translations.Models.Admin.Languages;

namespace Translations.Contracts.Services;

public interface ITranslationService
{
    Task<IEnumerable<TranslationLanguageDto>> GetTranslationLanguagesList();
    Task<JObject> GetAllTranslations();
    Task<JObject> GetTranslationByCode(string code);

}