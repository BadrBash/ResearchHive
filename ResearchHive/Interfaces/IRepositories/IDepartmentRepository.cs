using Model.Entities;
using ResearchHive.Wrapper;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<PaginatedResult<Department>> GetAllAsync(PaginationFilter filter);       
        Task<PaginatedResult<Department>> GetAllAsync(Expression<Func<Department, bool>> expression, PaginationFilter filter);
        Task<Department> GetAsync(Expression<Func<Department, bool>> expression);
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department> GetAsync(Guid id);
    }
}
