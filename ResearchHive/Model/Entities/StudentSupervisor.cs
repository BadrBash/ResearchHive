using Model.Common.Contracts;

namespace Model.Entities
{
    public class StudentSupervisor : BaseEntity
    {
        public StudentSupervisor(Guid studentId, Guid lecturerId)
        {
            StudentId = studentId;
            LecturerId = lecturerId;
        }

        public Guid StudentId { get; private set; }
        public Student Student { get; private set; }
        
        public Guid LecturerId { get; private set; }
        public Lecturer Lecturer { get; private set; }

    }
}
