using Model.Common.Contracts;

namespace Model.Entities
{
    public class UserRole : BaseEntity
    {
        public UserRole(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public Guid UserId {get; private set;}
        public User User {get;  set;}
        public Guid RoleId {get; private set;}
        public Role Role {get;  set;}
        

    }
}
