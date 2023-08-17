using Model.Common.Contracts;
using Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;
namespace Model.Entities
{
    public class Student : BaseEntity
    {
        public Student(Guid departmentId, Guid userId, string matricNumber, Level level)
        {
            DepartmentId = departmentId;
            UserId = userId;
            Projects = new HashSet<Project>();
            StudentSupervisors = new HashSet<StudentSupervisor>();
            MatricNumber = matricNumber;
            Level = level;
        }
        public Level Level { get; private set; }
        public bool IsActiveStudent { get; private set; }
        public Guid DepartmentId { get; private set; }
        public Department Department { get; private set; }
        public User User { get; private set; }
        [ForeignKey(nameof(User))]
        public Guid UserId { get; private set; }
        public ICollection<Project> Projects { get; private set; }
        public ICollection<StudentSupervisor> StudentSupervisors {get; private set;}
        public string MatricNumber { get; private set; }


        public Student DeactivateStudentship()
        {
            this.IsActiveStudent = false;
            return this;
        }

        public Student ActivateStudentship(Level level)
        {
            this.IsActiveStudent = true;
            Level = level;
            return this;

        }
    }
}
