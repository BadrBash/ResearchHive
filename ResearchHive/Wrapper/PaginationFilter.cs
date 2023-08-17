namespace ResearchHive.Wrapper;

public class PaginationFilter : BaseFilter
{
    public int PageNumber { get; set; }

    public string SearchValue { get; set; }
    public int PageSize { get; set; } 

    public string[]? OrderBy { get; set; }
}
