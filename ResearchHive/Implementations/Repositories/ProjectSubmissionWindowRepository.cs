using Application.Interfaces.Repositories;
using ResearchHive.Wrapper;
using Model.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;
using System.Linq;
using System.Linq.Expressions;

namespace Persistence.Repositories
{
    public class ProjectSubmissionWindowRepository : BaseRepository<ProjectSubmissionWindow>, IProjectSubmissionWindowRepository
    {
        private readonly ApplicationDbContext _context;
        public ProjectSubmissionWindowRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectSubmissionWindow>> GetAllAsync()
        {
            return await _context.ProjectSubmissionWindows.ToListAsync();
        }

        public async Task<PaginatedResult<ProjectSubmissionWindow>> GetAsync(PaginationFilter filter, Expression<Func<ProjectSubmissionWindow, bool>> expression)
        {
            return await _context.ProjectSubmissionWindows.Where(expression)
                .ToPaginatedResultListAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<IEnumerable<ProjectSubmissionWindow>> GetAsync(Expression<Func<ProjectSubmissionWindow, bool>> expression)
        {
            return await _context.ProjectSubmissionWindows.Where(expression).ToListAsync();
        }

        public async  Task<ProjectSubmissionWindow> GetByExpressionAsync(Expression<Func<ProjectSubmissionWindow, bool>> expression)
        {
            return await _context.ProjectSubmissionWindows.Include(psw => psw.Projects).Where(expression).FirstOrDefaultAsync();
        }
    }
}
