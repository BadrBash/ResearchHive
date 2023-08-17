using Application.DTOs.UserDtos;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Persistence.Context;
using Persistence.Extensions;
using Persistence.Repositories;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Linq.Expressions;

namespace ResearchHive.Implementations.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<User> GetAllUsers(Expression<Func<User, bool>> expression)
        {
            return  _context.Users.Where(expression);
        }

        public async Task<IEnumerable<User>> GetAllUsers(Expression<Func<User, bool>> expression, bool AsNoTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<User> query = _context.Users;
            if (AsNoTracking) query = query.AsNoTracking();
            return await query.Where(expression).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAllUsers(bool AsNoTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<User> query = _context.Users;
            if(AsNoTracking && query != null) query = query.AsNoTracking()
                    .Include(u => u.Student).
                Include(u => u.Administrator).
                Include(u => u.Lecturer);
            return await query.ToListAsync(cancellationToken);
        }
        

        public async Task<User> GetUser(Guid id)
        {
          
            return (await _context.Users.Include(u => u.Student).
                Include(u => u.Administrator).
                Include(u => u.Lecturer).FirstOrDefaultAsync(us => us.Id == id));
        }

        public async Task<User> GetUser(Expression<Func<User, bool>> expression, bool AsNoTracking = true, CancellationToken cancellationToken = default)
        {
           IQueryable<User> query =   (_context.Users);
            if (AsNoTracking) query = query.AsNoTracking();
            if (expression != null) query = query.Include(u => u.Student).
                Include(u => u.Administrator).
                Include(u => u.Lecturer)
                    .Where(expression);
            return await query.SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<string> GetUserNameById(Guid id)
        {
            return (await _context.Users.Include(u => u.Student).
                Include(u => u.Administrator).
                Include(u => u.Lecturer).FirstOrDefaultAsync(us => us.Id == id)).UserName;
        }

        public async Task<List<ManageUserRolesViewModel>> ManageUserRoles(Guid userId)
        {

            var user = await _context.Users.
                Include(u => u.Student).
                Include(u => u.Administrator).
                Include(u => u.Lecturer).
                Where(x => x.Id == userId).FirstOrDefaultAsync();

            var roles = new List<ManageUserRolesViewModel>();

            if (user != null)
            {
                roles = await _context.Roles.Select(x => new ManageUserRolesViewModel
                {
                    RoleId = x.Id,
                    RoleName = x.Name
                }).ToListAsync();

                foreach (var role in roles)
                {

                    var isRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == role.RoleId);

                    if (isRole != null)
                    {
                        role.Selected = true;
                    }
                    else
                    {
                        role.Selected = false;
                    }
                }
                return roles;
            }
            else
            {
                return roles;
            }

        }

       

        
        public async Task<PaginatedResult<UserDto>> LoadUsersAsync(string filter, PaginationFilter paginationFilter)
        {
            var query = _context.Users.Include(u => u.Student).
                Include(u => u.Administrator).
                Include(u => u.Lecturer)
                        .Where(c => filter == null || c.FirstName.Contains(filter)
                        || c.LastName.Contains(filter) || c.Email.Contains(filter)).OrderBy(c => c.FirstName);


            return await query.Select(c => new UserDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email
            })
                      .AsNoTracking()
                      .ToPaginatedResultListAsync(paginationFilter.PageNumber, paginationFilter.PageSize);

        }

        public new async Task<bool> ExistsAsync(Expression<Func<User, bool>> expression)
        {
            return await _context.Users.AnyAsync(expression);
        }

        public async Task<User> GetUserAsync(Expression<Func<User, bool>> expression)
        {
            return  await _context.Users.Include(u => u.Student).
                Include(u => u.Administrator).
                Include(u => u.Lecturer)
                        .Where(expression).FirstOrDefaultAsync();
        }
    }
}
