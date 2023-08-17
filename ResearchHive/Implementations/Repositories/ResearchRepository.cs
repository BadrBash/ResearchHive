using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Persistence.Context;
using Persistence.Extensions;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Linq.Expressions;

namespace Persistence.Repositories
{
    public class ResearchRepository : BaseRepository<Research>, IResearchRepository
    {
        private readonly ApplicationDbContext _context;
        public ResearchRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Research>> GetAllAsync()
        {
            return await _context.Researches.Include(r => r.Lecturer).ToListAsync();
        }

        public async Task<PaginatedResult<Research>> GetAsync(PaginationFilter filter, Expression<Func<Research, bool>> expression)
        {
            return await _context.Researches.Include(r => r.Lecturer)
                .Where(expression).ToPaginatedResultListAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<IEnumerable<Research>> GetAsync(Expression<Func<Research, bool>> expression)
        {
            return  await _context.Researches.Where(expression).ToListAsync();
        }

        public async Task<Research> GetByExpressionAsync(Expression<Func<Research, bool>> expression)
        {
            return await _context.Researches.Where(expression).SingleOrDefaultAsync();
        }
    }
}
