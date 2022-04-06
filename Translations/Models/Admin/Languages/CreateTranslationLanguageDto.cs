using System.ComponentModel.DataAnnotations;

namespace Translations.Models.Admin.Languages;

public class CreateTranslationLanguageDto
{
    [Display(Name = "Nazwa")]
    public string Name { get; set; }
    
    [Display(Name = "Nazwa angielska")]
    public string NameENG { get; set; }

    [Display(Name = "Kod")]
    public string Code { get; set; }

    [Display(Name = "Język")]
    public long LanguageId { get; set; }
}