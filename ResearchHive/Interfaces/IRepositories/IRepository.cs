using Model.Common.Contracts;
using System.Linq.Expressions;


namespace Application.Interfaces.Repositories
{
    public interface IRepository <T> where T : BaseEntity
    {
        Task<T> GetAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);

        IQueryable<T> Query();

        Task<T> AddAsync(T entity);

        Task<IEnumerable<T>> AddAsync(IEnumerable<T> entities);

        Task<T> UpdateAsync(T entity);

      /*  Task DeleteAsync(Guid id);*/
        void Remove(T entity);

        Task DeleteAsync(T entity);

        IQueryable<T> Query(Expression<Func<T, bool>> expression);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

}
