using ResearchHive.Wrapper;
using System.Linq.Expressions;
using Model.Entities;
using Application.Interfaces.Repositories;

namespace ResearchHive.Interfaces.Repositories
{
    public interface IResearchRepository : IRepository<Research>
    {
        Task<IEnumerable<Research>> GetAllAsync();
        Task<PaginatedResult<Research>> GetAsync(PaginationFilter filter, Expression<Func<Research, bool>> expression);
        Task<IEnumerable<Research>> GetAsync(Expression<Func<Research, bool>> expression);
        Task<Research> GetByExpressionAsync(Expression<Func<Research, bool>> expression);
    }
}
