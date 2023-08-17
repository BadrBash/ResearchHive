using Model.Common.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
namespace Model.Entities
{
    public class Research : BaseEntity
    {
        public Research(string title, string description, Guid authorId, string documentPath, string summary)
        {
            Title = title;
            Description = description;
            AuthorId = authorId;
            DocumentPath = documentPath;
            Summary = summary;
            IsApproved = false;
            Reviews = new HashSet<Review>();
        }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool IsApproved { get; private set; }
        public ICollection<Review> Reviews { get; private set; }
        public string DocumentPath { get; private set; }
        [ForeignKey(nameof(Lecturer))]

        public Guid AuthorId { get; private set; }
        public Lecturer Lecturer { get; private set; }
        public string Summary { get; private set; }

        public Research Update(string title, string description, string summary, string documentPath)
        {
            if(string.IsNullOrEmpty(title))
            {
                Title = Title;
            }
            if(string.IsNullOrEmpty(description))
            {
                Description = Description;
            }
            if(string.IsNullOrEmpty(summary))
            {
                Summary = Summary;
            }
            if(string.IsNullOrEmpty(documentPath))
            {
                DocumentPath = DocumentPath;
            }

            DocumentPath = documentPath;
            Summary  = summary; 
            Description = description; 
            Title = title;
            return this;

        }
       
    }
}
