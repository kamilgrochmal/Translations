namespace Domain.Translations;

public class TranslationRow
{
    public long Id { get; set; }

    public string Value { get; set; }

    public int TranslationLanguageId { get; set; }
    public TranslationLanguage TranslationLanguage { get; set; }

    public int TranslationKeyId { get; set; }
    public TranslationKey TranslationKey { get; set; }
}