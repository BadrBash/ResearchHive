
using Model.Entities;
using ResearchHive.Wrapper;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories
{
    public interface IAdministratorRepository : IRepository<Administrator>
    {
        Task<PaginatedResult<Administrator>> GetAllAsync(PaginationFilter filter, bool AsNoTracking = true);
        Task<PaginatedResult<Administrator>> GetAllAsync(PaginationFilter filter, Expression<Func<Administrator, bool>> expression, bool AsNoTracking = true);
        Task<Administrator> GetAsync(Expression<Func<Administrator, bool>> expression);
        Task<Administrator> GetAdministratorAsync(Guid id);
    }
}
