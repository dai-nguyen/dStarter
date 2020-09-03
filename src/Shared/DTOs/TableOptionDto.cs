namespace Shared.DTOs
{
    public class TableOptionDto
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public string[] SortBy { get; set; }
        public bool[] SortDesc { get; set; }
        public string[] GroupBy { get; set; }
        public bool[] GroupDesc { get; set; }
        public bool MultiSort { get; set; }
        public bool MustSort { get; set; }
    }
}
