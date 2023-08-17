using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Persistence.Context;
using Persistence.Extensions;
using ResearchHive.Wrapper;
using System.Linq.Expressions;

namespace Persistence.Repositories
{
    public class AdministratorRepository : BaseRepository<Administrator>, IAdministratorRepository
    {
        private readonly ApplicationDbContext _context;
        public AdministratorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Administrator> GetAdministratorAsync(Guid id)
        {
            IQueryable<Administrator> query = _context.Administrators;
            if (query != null) query = query.Include(l => l.User).Where(l => l.Id == id);
            return await query.SingleOrDefaultAsync();
        }

        public async Task<PaginatedResult<Administrator>> GetAllAsync(PaginationFilter filter, bool AsNoTracking = true)
        {
            IQueryable<Administrator> query = _context.Administrators;
            if (AsNoTracking && query != null) query.AsNoTracking().Include(l => l.User)
                     .Where(t => filter.SearchValue == null || ($"{t.User.FirstName} {t.User.LastName}".Contains(filter.SearchValue) || t.User.UserName.Contains(filter.SearchValue) || t.User.Email.Contains(filter.SearchValue)));
                   return await query.ToPaginatedResultListAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<PaginatedResult<Administrator>> GetAllAsync(PaginationFilter filter, Expression<Func<Administrator, bool>> expression, bool AsNoTracking = true)
        {
            IQueryable<Administrator> query = _context.Administrators;
            if (AsNoTracking && query != null)  query.AsNoTracking().Include(l => l.User)
                     .Where(t => filter.SearchValue == null || ($"{t.User.FirstName} {t.User.LastName}".Contains(filter.SearchValue) || t.User.UserName.Contains(filter.SearchValue) || t.User.Email.Contains(filter.SearchValue))).Where(expression);
                    return await query.ToPaginatedResultListAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<Administrator> GetAsync(Expression<Func<Administrator, bool>> expression)
        {
            IQueryable<Administrator> query = _context.Administrators;
            if (query != null) query = query.Include(l => l.User).Where(expression);
            return await query.FirstOrDefaultAsync();
        }

    }
}
