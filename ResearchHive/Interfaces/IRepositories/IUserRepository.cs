using Application.DTOs.UserDtos;
using Application.Interfaces.Repositories;
using Model.Entities;
using ResearchHive.Wrapper;
using System.Linq.Expressions;

namespace ResearchHive.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<string> GetUserNameById(Guid id);

        Task<User> GetUser(Guid id);
        IQueryable<User> GetAllUsers(Expression<Func<User, bool>> expression);
        Task<User> GetUserAsync(Expression<Func<User, bool>> expression);   
        Task<List<ManageUserRolesViewModel>> ManageUserRoles(Guid userId);
        Task<PaginatedResult<UserDto>> LoadUsersAsync(string filter, PaginationFilter paginationFilter);
        Task<IEnumerable<User>> GetAllUsers(Expression<Func<User, bool>> expression, bool AsNoTracking = true, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> GetAllUsers(bool AsNoTracking = true, CancellationToken cancellationToken = default);
        Task<User> GetUser(Expression<Func<User, bool>> expression, bool AsNoTracking = true, CancellationToken cancellationToken = default);
        
    }
}
