namespace Domain.Translations;

public class TranslationKey 
{
    public long Id { get; set; }

    public string Code { get; set; }
    public string Description { get; set; }

    public bool IsBackend { get; set; }
    public bool IsFrontend { get; set; }

    public int TranslationCategoryId { get; set; }
    public TranslationCategory TranslationCategory { get; set; }
        
    public virtual ICollection<TranslationRow> Translations { get; set; }
}