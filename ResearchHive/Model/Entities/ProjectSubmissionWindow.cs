using Model.Common.Contracts;
using Model.Constants;
using Model.Extensions;
using Model.Enums;

namespace Model.Entities
{
    public class ProjectSubmissionWindow : BaseEntity
    {
        
        public ProjectSubmissionWindow(int submissionYear, Level level, DateTime startDate,
            DateTime endDate, int weeksGrace, int numberOfChapters, string departmentName)
        {
            Set = $"R-H/{SubmissionYear}/{Level.GetDescription()}";
            SubmissionYear = submissionYear;
            Level = level;
            StartDate = startDate;
            EndDate = endDate;
            WeeksGrace = weeksGrace;
            Projects = new HashSet<Project>();
            NumberOfChapters = numberOfChapters;
            DepartmentName = departmentName;

            if(NumberOfChapters <= 0 || WeeksGrace <= 0)
            {
                throw new ArgumentException(ExceptionMessage.CannotBeZero);
            }

            if(SubmissionYear <= 0 || SubmissionYear < 2000)
            {
                throw new ArgumentException(ExceptionMessage.CannotBeZero + $"  or submission year cannot be less than 2000");
            }
        }

        public string Set { get; private set; }
        public int SubmissionYear { get; private set; }
        public string DepartmentName { get; private set; }  
        public Level Level { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set;}
        public bool IsClosed { get; private set; }
        public int NumberOfChapters { get; private set; }   
        public int WeeksGrace { get; private set; }
        public ICollection<Project> Projects { get; private set; }   
        public ProjectSubmissionWindow Update(int submissionYear, int weeksGrace, int numberOfChapters)
        {
            SubmissionYear = submissionYear;
            WeeksGrace = weeksGrace;
            NumberOfChapters = numberOfChapters;
            return this;
        }

        public ProjectSubmissionWindow CloseProject()
        {
            IsClosed = true;
            return this;
        }
    }
}
