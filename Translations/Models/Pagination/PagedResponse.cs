namespace Translations.Models.Pagination;

public class PagedResponse<T>
{
    public PagedResponse()
    {
        PagedData = new PagedData();
    }

    public PagedResponse(IEnumerable<T> data)
    {
        Data = data;
        PagedData = new PagedData();
    }

    public IEnumerable<T> Data { get; set; }
    public PagedData PagedData { get; set; }
}