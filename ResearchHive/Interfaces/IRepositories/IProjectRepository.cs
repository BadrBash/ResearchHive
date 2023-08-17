using Model.Entities;
using ResearchHive.Wrapper;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<PaginatedResult<Project>> GetAsync(PaginationFilter filter, Expression<Func<Project, bool>> expression);
        Task<IEnumerable<Project>> GetAsync(Expression<Func<Project, bool>> expression);
        Task<Project> GetByExpressionAsync(Expression<Func<Project, bool>> expression);

    }
}
