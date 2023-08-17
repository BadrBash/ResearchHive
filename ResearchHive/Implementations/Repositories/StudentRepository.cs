using Application.Interfaces.Repositories;
using ResearchHive.Wrapper;
using ResearchHive.Wrapper.Paging;
using Model.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        public StudentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students.Include(st => st.Department).
                Include(st => st.StudentSupervisors)
                .Include(st => st.User).ToListAsync();
        }

        public async Task<PaginatedResult<Student>> GetAsync(PaginationFilter filter, Expression<Func<Student, bool>> expression)
        {
            return await _context.Students.Include(st => st.Department).
                  Include(st => st.StudentSupervisors)
                 .Include(st => st.User).ToPaginatedResultListAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<IEnumerable<Student>> GetAsync(Expression<Func<Student, bool>> expression)
        {
            return await _context.Students.Include(st => st.Department).
                Include(st => st.StudentSupervisors)
                .Include(st => st.User).Where(expression).ToListAsync();
        }

        public async Task<Student> GetByExpressionAsync(Expression<Func<Student, bool>> expression)
        {
            return await _context.Students.Include(st => st.Department).
                  Include(st => st.StudentSupervisors)
                 .Include(st => st.User).Where(expression).SingleOrDefaultAsync();
        }
    }
}
