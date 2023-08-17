
using Model.Common.Contracts;
using Model.Constants;
using Model.ModelException;

namespace Model.Entities
{
    public class Lecturer : BaseEntity
    {
        public Lecturer(Guid userId, string staffNumber)
        {
            LecturerDepartments = new HashSet<LecturerDepartment>();
            Researches = new HashSet<Research>();
            UserId = userId;   
            StaffNumber = staffNumber;
            StudentSupervisors = new HashSet<StudentSupervisor>();
            if (string.IsNullOrEmpty(StaffNumber))
            {
                throw new ValueCannotBeNullException(ExceptionMessage.CannotBeNull);
            }
        }
        public string StaffNumber { get; private set; } 
        public User User { get; private set; }
        public Guid UserId { get; private set; }    

        public ICollection<LecturerDepartment> LecturerDepartments { get; private set; }
        public ICollection<StudentSupervisor> StudentSupervisors { get; private set; }
        public ICollection<Research> Researches { get; private set; }

      
    }
}
