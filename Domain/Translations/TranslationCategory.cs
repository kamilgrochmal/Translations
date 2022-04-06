namespace Domain.Translations;

public class TranslationCategory 
{
    public long Id { get; set; }
    public string Name{ get; set; }
    public string Description { get; set; }

    public virtual ICollection<TranslationKey> TranslationKeys { get; set; }
}