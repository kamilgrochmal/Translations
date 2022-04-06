using System.ComponentModel.DataAnnotations;

namespace Translations.Models.Admin.Categories;

public class CreateTranslationCategoryDto
{
    [Display(Name = "Nazwa")]
    public string Name { get; set; }

    [Display(Name = "Opis")]
    public string Description { get; set; }
}