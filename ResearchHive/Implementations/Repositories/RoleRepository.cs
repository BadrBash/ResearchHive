using Model.Entities;
using Persistence.Context;
using Persistence.Repositories;
using ResearchHive.Interfaces.Repositories;

namespace ResearchHive.Implementations.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
