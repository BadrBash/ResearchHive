using Application.Interfaces.Repositories;
using ResearchHive.Wrapper;
using Model.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;
using System.Linq.Expressions;

namespace Persistence.Repositories
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _context.Reviews.Include(r => r.Research)
                .Include(r => r.Project).ToListAsync();
        }

        public async Task<PaginatedResult<Review>> GetAsync(PaginationFilter filter, Expression<Func<Review, bool>> expression)
        {
            return await _context.Reviews.Include(r => r.Research)
                .Include(r => r.Project).Where(expression)
                .ToPaginatedResultListAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<IEnumerable<Review>> GetAsync(Expression<Func<Review, bool>> expression)
        {
            return await _context.Reviews.Include(r => r.Research)
                .Include(r => r.Project)
                .Where(expression)
                .ToListAsync();
        }

        public async Task<Review> GetByExpressionAsync(Expression<Func<Review, bool>> expression)
        {
            return await _context.Reviews.Include(r => r.Research)
                .Include(r => r.Project).SingleOrDefaultAsync();
        }
    }
}
