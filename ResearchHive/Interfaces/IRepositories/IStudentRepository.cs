using Model.Entities;
using ResearchHive.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<PaginatedResult<Student>> GetAsync(PaginationFilter filter, Expression<Func<Student, bool>> expression);
        Task<IEnumerable<Student>> GetAsync(Expression<Func<Student, bool>> expression);
        Task<Student> GetByExpressionAsync(Expression<Func<Student, bool>> expression);
    }
}
