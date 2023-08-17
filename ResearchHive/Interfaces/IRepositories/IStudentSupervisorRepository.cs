using ResearchHive.Wrapper;
using Model.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories
{
    public interface IStudentSupervisorRepository : IRepository<StudentSupervisor>
    {
        Task<IEnumerable<StudentSupervisor>> GetAllAsync();
        Task<PaginatedResult<StudentSupervisor>> GetAsync(PaginationFilter filter, Expression<Func<StudentSupervisor, bool>> expression);
        Task<IEnumerable<StudentSupervisor>> GetAsync(Expression<Func<StudentSupervisor, bool>> expression);
        Task<StudentSupervisor> GetByExpressionAsync(Expression<Func<StudentSupervisor, bool>> expression);
    }
}
