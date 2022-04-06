using System.Linq.Dynamic.Core;
using Translations.Models.Pagination;

namespace Translations.Helpers.PaginationExtensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ToFilterView<T>(this IQueryable<T> query, PaginationFilter filter)
        {
            // filter
            //query = Filter(query, filter.Filter);
            //sort
            if (filter.SortField != null && filter.SortDir != null)
            {
                if (!String.Equals(filter.SortDir, "DESC", StringComparison.CurrentCultureIgnoreCase) && !String.Equals(filter.SortDir, "ASC", StringComparison.CurrentCultureIgnoreCase))
                    filter.SortDir = "ASC";
                
                List<Sort> sortList = new List<Sort>();
                if(string.IsNullOrEmpty(filter.SortDir) && string.IsNullOrEmpty(filter.SortField))
                {
                    var defaultsort = new Sort() { Field = "Id", Dir = "DESC" };
                    sortList.Add(defaultsort);
                }
                else
                {
                    var defaultsort = new Sort() { Field = filter.SortField, Dir = filter.SortDir };
                    sortList.Add(defaultsort);
                }
                query = Sort(query, filter.SortField, filter.SortDir);
                // EF does not apply skip and take without order
                //query = Limit(query, filter.PageSize, filter.PageNumber);
            }
            else
            {
                var defaultsort = new Sort() { Field = "Id", Dir = "DESC" };
                List<Sort> sortList = new List<Sort> {defaultsort};
                query = Sort(query, filter.SortField, filter.SortDir);
            }
            return query;
        }

        public static IQueryable<T> ToPaginationView<T>(this IQueryable<T> query, PaginationFilter filter)
        {
            // EF does not apply skip and take without order
            query = Limit(query, filter.PageSize, filter.PageNumber);
            return query;
        }

        private static IQueryable<T> Filter<T>(IQueryable<T> queryable, Filter filter)
        {
            if ((filter != null) && filter.Filters.Any())
            {
                var filters = GetAllFilters(filter);
                var values = filters.Select(f => f.Value).ToArray();
                var where = Transform(filter, filters);
                queryable = queryable.Where(where, values);
            }
            return queryable;
        }

        private static IQueryable<T> Sort<T>(IQueryable<T> queryable, string sortField, string sortDir)
        {
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortDir))
            {
                if (string.Equals(sortField, "Surname", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (string.Equals(sortDir, "ASC", StringComparison.CurrentCultureIgnoreCase))
                    {
                        queryable = queryable.OrderBy("Surname ASC, Name ASC");
                    }
                    else
                    {
                        queryable = queryable.OrderBy("Surname DESC, Name DESC");
                    }
                }else if (string.Equals(sortField, "PlayerProfile.Surname", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (string.Equals(sortDir, "ASC", StringComparison.CurrentCultureIgnoreCase))
                    {
                        queryable = queryable.OrderBy("PlayerProfile.Surname ASC, PlayerProfile.Name ASC");
                    }
                    else
                    {
                        queryable = queryable.OrderBy("PlayerProfile.Surname DESC, PlayerProfile.Name DESC");
                    } 
                }else if (string.Equals(sortField, "TrainerProfile.Surname", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (string.Equals(sortDir, "ASC", StringComparison.CurrentCultureIgnoreCase))
                    {
                        queryable = queryable.OrderBy("TrainerProfile.Surname ASC, TrainerProfile.Name ASC");
                    }
                    else
                    {
                        queryable = queryable.OrderBy("TrainerProfile.Surname DESC, TrainerProfile.Name DESC");
                    } 
                }
                else
                {
                    var ordering =  sortField + " " + sortDir;
                    queryable = queryable.OrderBy(ordering);
                }
            }
            else
            {
                var ordering =  "Id" + " " + "DESC";
                queryable = queryable.OrderBy(ordering);
            }
            return queryable;
        }

        private static IQueryable<T> Limit<T>(IQueryable<T> queryable, int limit, int offset)
        {
            int skipRecords = (offset - 1) * limit;
            return queryable.Skip(skipRecords).Take(limit);
        }

        private static readonly IDictionary<string, string>
        Operators = new Dictionary<string, string>
        {
            {"eq", "="},
            {"neq", "!="},
            {"lt", "<"},
            {"lte", "<="},
            {"gt", ">"},
            {"gte", ">="},
            {"startswith", "StartsWith"},
            {"endswith", "EndsWith"},
            {"contains", "Contains"},
            {"doesnotcontain", "Contains"},
        };

        public static IList<Filter> GetAllFilters(Filter filter)
        {
            var filters = new List<Filter>();
            GetFilters(filter, filters);
            return filters;
        }

        private static void GetFilters(Filter filter, IList<Filter> filters)
        {
            if (filter.Filters != null && filter.Filters.Any())
            {
                foreach (var item in filter.Filters)
                {
                    GetFilters(item, filters);
                }
            }
            else
            {
                filters.Add(filter);
            }
        }

        public static string Transform(Filter filter, IList<Filter> filters)
        {
            if (filter.Filters != null && filter.Filters.Any())
            {
                return "(" + String.Join(" " + filter.Logic + " ",
                    filter.Filters.Select(f => Transform(f, filters)).ToArray()) + ")";
            }
            int index = filters.IndexOf(filter);
            var comparison = Operators[filter.Operator];
            if (filter.Operator == "doesnotcontain")
            {
                return String.Format("({0} != null && !{0}.ToString().{1}(@{2}))",
                    filter.Field, comparison, index);
            }
            if (comparison == "StartsWith" ||
                comparison == "EndsWith" ||
                comparison == "Contains")
            {
                return String.Format("({0} != null && {0}.ToString().{1}(@{2}))",
                filter.Field, comparison, index);
            }
            return String.Format("{0} {1} @{2}", filter.Field, comparison, index);
        }
    }
}