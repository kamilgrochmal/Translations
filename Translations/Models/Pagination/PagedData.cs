namespace Translations.Models.Pagination;


//For now pagination is here, after approvement can be moved; 
public class PagedData
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public int? ResultsCount { get; set; }
    public int? Pages { get; set; }
}