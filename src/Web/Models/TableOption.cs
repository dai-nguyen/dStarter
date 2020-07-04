using System.Collections.Generic;

namespace Web.Models
{
    public class TableOption
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public IEnumerable<string> SortBy { get; set; }
        public IEnumerable<bool> SortDesc { get; set; }
        public IEnumerable<string> GroupBy { get; set; }
        public IEnumerable<bool> GroupDesc { get; set; }
        public bool MultiSort { get; set; }
        public bool MustSort { get; set; }
    }
}
