namespace Translations.Models.Admin.Keys;

public class TranslationKeyDto  
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

    public bool IsBackend { get; set; }
    public bool IsFrontend { get; set; }

    public string CategoryName { get; set; }
    public int CategoryId { get; set; }
    public List<TranslationLanguagesWithTranslationsDto> LanguagesWithTranslations { get; set; } = new();
    public ICollection<string> TranslationsInLanguages { get; set; } = new List<string>();

}

public class TranslationLanguagesWithTranslationsDto
{
    public int TranslationLanguageId { get; set; }
    public string TranslationLanguageName { get; set; }
    public string TranslationValue { get; set; }
    public int? TranslationValueId { get; set; }
    
}