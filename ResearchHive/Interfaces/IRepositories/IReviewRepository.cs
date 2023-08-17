using Model.Entities;
using ResearchHive.Wrapper;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetAllAsync();
        Task<PaginatedResult<Review>> GetAsync(PaginationFilter filter, Expression<Func<Review, bool>> expression);
        Task<IEnumerable<Review>> GetAsync(Expression<Func<Review, bool>> expression);
        Task<Review> GetByExpressionAsync(Expression<Func<Review, bool>> expression);
    }
}
