namespace Infrastructure.Specifications
{
    public class RoleSpecification
    {
        public string Search { get; private set; }
        public string SortBy { get; private set; }
        public string SortDirection { get; private set; }
        public int PageSize { get; private set; }
        public int PageNumber { get; private set; }

        public RoleSpecification(
            string search = "",
            string sortBy = "",
            string sortDir = "",
            int pageSize = 0,
            int pageNumber = 0
            )
        {
            Search = search;
            SortBy = sortBy;
            SortDirection = sortDir;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}
