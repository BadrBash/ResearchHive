using Model.Common.Contracts;
using Model.Constants;
using Model.ModelException;
using Model.Enums;

namespace Model.Entities
{
    public class Project : BaseEntity
    {
     
        public Project(string title, string description, string summary, Guid studentId,
            Guid projectSubmissionWindowId, ProjectStage projectStage)
        {
            Title = title;
            Description = description;
            Summary = summary;
            StudentId = studentId;
            ProjectStage  = projectStage;
            ProjectDocuments = new HashSet<ProjectDocument>();
            ProjectSubmissionWindowId = projectSubmissionWindowId;
            IsApproved = false;
            Reviews = new HashSet<Review>();
            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Description) || string.IsNullOrEmpty(Summary))
            {
                throw new ValueCannotBeNullException(ExceptionMessage.CannotBeNull);
            }
        }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Summary { get; private set; }
        public Guid StudentId { get; private set; }
        public ProjectStage ProjectStage { get; private set; } 
        public Student Student { get; private set; }
        public ICollection<ProjectDocument> ProjectDocuments { get; private set;}
        public Guid ProjectSubmissionWindowId { get; private set; }
        public bool IsApproved { get; private set; }
        public ProjectSubmissionWindow ProjectSubmissionWindow { get; private set;  }
        public bool IsComplete { get; private set; }
        public ICollection<Review> Reviews { get; private set; }
        public Project Approve()
        {
            if (IsComplete && ProjectStage == ProjectStage.AwaitingApproval)
            {
                IsApproved = true;
                ProjectStage = ProjectStage.CompletedAndApproved;
                return this;
            }
            IsApproved = false;
            return this;
        }
        public Project SetInProgress()
        {
            ProjectStage = ProjectStage.InProgress;
            return this;
        }

        public Project CompleteSubmission()
        {
           if((ProjectDocuments?.Count == ProjectSubmissionWindow?.NumberOfChapters || ProjectDocuments?.LastOrDefault()?.ChapterNumber == ProjectSubmissionWindow?.NumberOfChapters)
                && ProjectStage == ProjectStage.InProgress)
           {
                ProjectStage = ProjectStage.AwaitingApproval;
                IsComplete = true;
                return this;
           }
           IsComplete = false;
            return this;
        }
    }
}
