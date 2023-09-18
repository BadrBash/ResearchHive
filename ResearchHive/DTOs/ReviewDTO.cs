using Model.Entities;

namespace ResearchHive.DTOs
{
    public class ReviewDTO
    {

        public string? Comment { get;  set; }
        public bool Liked { get;  set; }
        public Guid? ProjectId { get;  set; }
        public string ? ProjectName { get;  set; }   
        public Guid? ResearchId { get;  set; }
    }
}
