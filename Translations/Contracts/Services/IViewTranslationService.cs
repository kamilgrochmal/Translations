namespace Translations.Contracts.Services;

public interface IViewTranslationService
{
    Task<string> Get(string key, string category = null);
}