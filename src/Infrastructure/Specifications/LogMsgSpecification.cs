using System;

namespace Infrastructure.Specifications
{
    public class LogMsgSpecification
    {
        public string[] UserNames { get; private set; }
        public DateTime Date { get; private set; }
        public string[] Levels { get; private set; }
        public string Search { get; private set; }
        public string SortBy { get; private set; }
        public string SortDirection { get; private set; }
        public int PageSize { get; private set; }
        public int PageNumber { get; private set; }

        public LogMsgSpecification(
            string[] usernames,
            DateTime date,
            string[] levels,
            string search = "",
            string sortBy = "",
            string sortDir = "",
            int pageSize = 0,
            int pageNumber = 0
            )
        {
            UserNames = usernames;
            Date = date;
            Levels = levels;
            Search = search;
            SortBy = sortBy;
            SortDirection = sortDir;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}
