using Application.Interfaces.Repositories;
using ResearchHive.Wrapper;
using Model.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;
using System.Linq.Expressions;

namespace Persistence.Repositories
{
    public class LecturerRepository : BaseRepository<Lecturer>, ILecturerRepository
    {
        private readonly ApplicationDbContext _context;
        public LecturerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<Lecturer>> GetAllAsync(PaginationFilter filter)
        {
            return await _context.Lecturers.Include(l => l.User).ToPaginatedResultListAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<PaginatedResult<Lecturer>> GetAllAsync(PaginationFilter filter, Expression<Func<Lecturer, bool>> expression)
        {
            return await _context.Lecturers.Include(l => l.User).Where(expression).ToPaginatedResultListAsync(filter.PageNumber, 
                filter.PageSize);
        }

        public async Task<Lecturer> GetAsync(Expression<Func<Lecturer, bool>> expression)
        {
            return await _context.Lecturers.Include(l => l.User).Where(expression).SingleOrDefaultAsync();
        }
    }
}
