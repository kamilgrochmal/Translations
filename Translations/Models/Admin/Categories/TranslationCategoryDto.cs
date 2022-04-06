using System.ComponentModel.DataAnnotations;

namespace Translations.Models.Admin.Categories;

public class TranslationCategoryDto
{
    public int Id { get; set; }
    [Display(Name = "Nazwa")] public string Name { get; set; }

    [Display(Name = "Opis")] public string Description { get; set; }

    public ICollection<TranslationKeysWithLanguagesDto> TranslationKeys { get; set; } = new List<TranslationKeysWithLanguagesDto>();
    
}
public class TranslationKeysWithLanguagesDto
{
    public string TranslationKey { get; set; }
    public int TranslationKeyId { get; set; }
    public int TranslationCategoryId { get; set; }
    public List<LanguageDto> TranslatedLanguages { get; set; } = new();
}

public class LanguageDto
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}

