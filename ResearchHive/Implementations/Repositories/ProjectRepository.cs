using Application.Interfaces.Repositories;
using ResearchHive.Wrapper;
using Model.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;
using System.Linq.Expressions;

namespace Persistence.Repositories
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        private readonly ApplicationDbContext _context;
        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects.
                 Include(p => p.ProjectSubmissionWindow).ToListAsync();

        }

        public async Task<PaginatedResult<Project>> GetAsync(PaginationFilter filter, Expression<Func<Project, bool>> expression)
        {
            return await _context.Projects.Include(p => p.ProjectSubmissionWindow)
                 .Where(expression).ToPaginatedResultListAsync(filter.PageSize, filter.PageNumber);
        }

        public async Task<IEnumerable<Project>> GetAsync(Expression<Func<Project, bool>> expression)
        {
            return await _context.Projects
                .Include(p => p.ProjectSubmissionWindow)
                .Where(expression).ToListAsync();
        }

        public async Task<Project> GetByExpressionAsync(Expression<Func<Project, bool>> expression)
        {
            return await _context.Projects
                 .Include(p => p.ProjectSubmissionWindow).Where(expression)
                 .SingleOrDefaultAsync();
        }
    }
}
