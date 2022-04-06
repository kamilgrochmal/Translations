namespace Translations.Models.Pagination
{
    public class PaginationFilter
    {
        protected string _sortField;
        public string SortField
        {
            get => _sortField;
            set => _sortField = value;
        }
        
        protected string _sortDir;
        public string SortDir
        {
            get => _sortDir;
            set => _sortDir = value;
        }
        
        //public List<Sort> Sort { get; set; } = new List<Sort>();
        //public Filter Filter { get; set; }
        public List<FilterItem> Filters { get; set; } = new List<FilterItem>();

        private int _pageNumber;
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = (value > 0) ? value : 1;
        }

        private int _pageSize;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > 0) ? value : 25;
        }

        public PaginationFilter()
        {
            _pageNumber = 1;
            _pageSize = 25;
        }
        
        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public PaginationFilter(int pageNumber, int pageSize, string sortField, string sortDir)
        {
            _pageNumber = pageNumber;
            _pageSize = pageSize;
            SortField = sortField;
            SortDir = sortDir;
        }

        public PaginationFilter(int pageNumber, int pageSize, string sortField, string sortDir, List<FilterItem> filters)
        {
            _pageNumber = pageNumber;
            _pageSize = pageSize;
            SortField = sortField;
            SortDir = sortDir;
            Filters = filters;
        }
    }
}