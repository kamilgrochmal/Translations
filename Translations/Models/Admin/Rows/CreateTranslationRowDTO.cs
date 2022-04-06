using System.ComponentModel.DataAnnotations;

namespace Translations.Models.Admin.Rows;

public class CreateTranslationRowDto
{

    [Display(Name = "Wartość")]
    public string Value { get; set; }

    [Display(Name = "Język tłumaczenia")]
    public int TranslationLanguageId { get; set; }

    [Display(Name = "Klucz")]
    public int TranslationKeyId { get; set; }
    [Display(Name = "Język tłumaczenia")]

    public string TranslationLanguage { get; set; }
    [Display(Name = "Klucz")]

    public string TranslationKey { get; set; }
        
}




