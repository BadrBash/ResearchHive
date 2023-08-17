using Model.Common.Contracts;

namespace Model.Entities
{
    public class LecturerDepartment : BaseEntity
    {
        public LecturerDepartment(Guid lecturerId, Guid departmentId)
        {
            LecturerId = lecturerId;
            DepartmentId = departmentId;
        }

        public Guid LecturerId { get; private set; }
        public Lecturer Lecturer { get; private set; }
        public Guid DepartmentId{ get; private set; }
        public Department Department { get; private set; }
    }
}
