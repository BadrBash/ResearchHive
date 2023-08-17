using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Common.Contracts;
using Persistence.Context;
using System.Linq.Expressions;

namespace Persistence.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>()
             .AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>()
            .AddRangeAsync(entities);
            return entities;
        }


        public  async Task DeleteAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            entity.IsDeleted = true;
            await Task.CompletedTask;
        }

       

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Set<T>().AnyAsync(t => t.Id == id);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().AnyAsync(expression);
        }

       

        public async Task<T> GetAsync(Guid id)
        {
            return await _context.Set<T>().Where(t => t.Id == id).SingleOrDefaultAsync();
        }


        public IQueryable<T> Query()
        {
            return _context.Set<T>()
                .AsQueryable();
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>()
                .Where(expression);
        }

        public void Remove(T entity)
        {
             _context.Set<T>()
                 .Remove(entity);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<T> UpdateAsync(T entity)
        {
              _context.Update(entity);
            return entity;
        }

        
    }
}
