
using Model.Common.Contracts;

namespace Model.Entities
{
    public class Role : BaseEntity
    {
        public Role(string name)
        {
            Name = name;
            UserRoles =  new HashSet<UserRole>();
        }

        public string Name {get;private set;}
        public ICollection<UserRole> UserRoles {get; set;} = new HashSet<UserRole>();
        
    }
}