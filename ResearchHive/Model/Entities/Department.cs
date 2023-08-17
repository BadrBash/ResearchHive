
using Model.ModelException;
using Model.Common.Contracts;
using Model.Constants;

namespace Model.Entities
{
    public class Department : BaseEntity
    {

        public Department(string name, string description)
        {
            Name = name;
            Description = description;
            LecturerDepartments = new HashSet<LecturerDepartment>();
            Students = new HashSet<Student>();
            if(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Description))
            {
                throw new ValueCannotBeNullException(ExceptionMessage.CannotBeNull);
            }
        }

        public string Name { get; private set; }
        public ICollection<Student> Students { get; private set; }
        public string Description { get; private set; } 
        public ICollection<LecturerDepartment> LecturerDepartments { get; private set; } 

        public Department Update(string name, string description)
        {
            Name = name;
            Description = description;
            return this;
        }

        
    }
}
