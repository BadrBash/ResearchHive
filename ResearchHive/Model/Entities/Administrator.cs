using Model.Common.Contracts;

namespace Model.Entities
{
    public class Administrator : BaseEntity
    {
        public Administrator(Guid userId)
        {
            UserId = userId;
        }

        public User? User { get; set; }
        public Guid UserId { get; private set; }

        
    }
}
