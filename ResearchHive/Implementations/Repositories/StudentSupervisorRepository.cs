using Application.Interfaces.Repositories;
using ResearchHive.Wrapper;
using Model.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;
using System.Linq.Expressions;


namespace Persistence.Repositories
{
    public class StudentSupervisorRepository : BaseRepository<StudentSupervisor>, IStudentSupervisorRepository
    {
        private readonly ApplicationDbContext _context;
        public StudentSupervisorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentSupervisor>> GetAllAsync()
        {
            return await _context.StudentSupervisors
                .Include(ss => ss.Student).
                Include(ss => ss.Lecturer).
                ToListAsync();
        }

        public async Task<PaginatedResult<StudentSupervisor>> GetAsync(PaginationFilter filter, Expression<Func<StudentSupervisor, bool>> expression)
        {
           return await _context.StudentSupervisors.Include(ss => ss.Student).
                Include(ss => ss.Lecturer).
                ToPaginatedResultListAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<IEnumerable<StudentSupervisor>> GetAsync(Expression<Func<StudentSupervisor, bool>> expression)
        {
            return await _context.StudentSupervisors.Include(ss => ss.Student).
                Include(ss => ss.Lecturer).
                Where(expression).
                ToListAsync();
        }

        public async Task<StudentSupervisor> GetByExpressionAsync(Expression<Func<StudentSupervisor, bool>> expression)
        {
           return await _context.StudentSupervisors.Include(ss => ss.Student).
                Include(ss => ss.Lecturer).
                Where(expression).
                SingleOrDefaultAsync();
        }
    }
}
