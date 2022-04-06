using System.ComponentModel.DataAnnotations;

namespace Translations.Models.Admin.Keys;

public class CreateTranslationKeyDto
{
    [Display(Name = "Klucz")]
    public string Code { get; set; }

    [Display(Name = "Opis")]
    public string Description { get; set; }

    public bool IsBackend { get; set; }
    public bool IsFrontend { get; set; }

    [Display(Name = "Kategoria")]
    public int TranslationCategoryId { get; set; }

}