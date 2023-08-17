namespace ResearchHive.Wrapper.Paging
{
    public class PaginationMeta
    {
        public PaginationMeta()
        { }



        public PaginationMeta(int currentPage, int currentPageSize, int totalPages, int totalRecords)
        {
            CurrentPage = currentPage;
            CurrentPageSize = currentPageSize;
            TotalPages = totalPages;
            TotalRecords = totalRecords;
        }



        public int CurrentPage { get; }

        public int CurrentPageSize { get; }

        public int TotalPages { get; }

        public int TotalRecords { get; }
    }
}