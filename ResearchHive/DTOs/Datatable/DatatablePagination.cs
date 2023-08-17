namespace Application.DTOs.Datatable
{
    public class DatatablePagination
    {
        public string? Field { get; set; }
        public int Page { get; set; } = 1;
        public int Pages { get; set; } = 1;
        public int PerPage { get; set; } = 40;
        public DatatableSortOption? Sort { get; set; }
        public int Total { get; set; } = 40;
    }
}