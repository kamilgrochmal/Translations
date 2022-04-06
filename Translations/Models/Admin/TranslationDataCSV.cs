using CsvHelper.Configuration.Attributes;

namespace Translations.Models.Admin;

public class TranslationDataCsv
{
    [Name("TRANSLATION_CODE")]
    public string Code { get; set; }
        
    [Name("pl")]
    public string LangPl { get; set; }
        
    [Name("en")]
    public string LangGb { get; set; }
        
    [Name("de")]
    public string LangDe { get; set; }
        
    [Name("it")]
    public string LangIt { get; set; }
        
    [Name("fr")]
    public string LangFr { get; set; }
        
    [Name("es")]
    public string LangEs { get; set; }
        
    [Name("hu")]
    public string LangHu { get; set; }
        
    [Name("lt")]
    public string LangLt { get; set; }
        
    [Name("ua")]
    public string LangUa { get; set; }
}