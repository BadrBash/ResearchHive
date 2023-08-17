using ResearchHive.Wrapper;
using Model.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories
{
    public interface ILecturerRepository : IRepository<Lecturer>
    {
        Task<PaginatedResult<Lecturer>> GetAllAsync(PaginationFilter filter); 
        Task<PaginatedResult<Lecturer>> GetAllAsync(PaginationFilter filter, Expression<Func<Lecturer, bool>> expression);
        Task<Lecturer> GetAsync(Expression<Func<Lecturer, bool>> expression);
        Task<Lecturer> GetAsync(Guid id);
    }
}
