using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Persistence.Context;
using Persistence.Extensions;
using ResearchHive.Wrapper;
using System.Linq.Expressions;

namespace Persistence.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<Department>> GetAllAsync(PaginationFilter filter)
        {
            return await _context.Departments.Where(dept => filter.SearchValue == null || dept.Name.Contains(filter.SearchValue) || dept.Description.Contains(filter.SearchValue)).ToPaginatedResultListAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<PaginatedResult<Department>> GetAllAsync(Expression<Func<Department, bool>> expression, PaginationFilter filter)
        {
            return await _context.Departments.Where(expression).
                Where(dept => filter.SearchValue == null || dept.Name.Contains(filter.SearchValue) || dept.Description.Contains(filter.SearchValue)).ToPaginatedResultListAsync(filter.PageNumber, filter.PageSize);
        }

        public  async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department> GetAsync(Expression<Func<Department, bool>> expression)
        {
            return await _context.Departments.Where(expression).FirstOrDefaultAsync();
        }
    }
}
