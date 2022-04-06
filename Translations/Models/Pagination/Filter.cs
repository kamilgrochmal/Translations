namespace Translations.Models.Pagination;

public class Filter
{
    public string Field { get; set; }
    public string Operator { get; set; }
    public object Value { get; set; }
    public string Logic { get; set; }
    public ICollection<Filter> Filters { get; set; } = new List<Filter>();
}