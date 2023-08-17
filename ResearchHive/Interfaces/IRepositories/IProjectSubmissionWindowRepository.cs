using Model.Entities;
using ResearchHive.Wrapper;
using System.Linq.Expressions;
namespace Application.Interfaces.Repositories
{
    public interface IProjectSubmissionWindowRepository : IRepository<ProjectSubmissionWindow>
    {
        Task<IEnumerable<ProjectSubmissionWindow>> GetAllAsync();
        Task<PaginatedResult<ProjectSubmissionWindow>> GetAsync(PaginationFilter filter, Expression<Func<ProjectSubmissionWindow, bool>> expression);
        Task<IEnumerable<ProjectSubmissionWindow>> GetAsync(Expression<Func<ProjectSubmissionWindow, bool>> expression);
        Task<ProjectSubmissionWindow> GetByExpressionAsync(Expression<Func<ProjectSubmissionWindow, bool>> expression);
    }
}
