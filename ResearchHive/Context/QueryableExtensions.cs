using Microsoft.EntityFrameworkCore;
using ResearchHive.Wrapper;
using System.Linq.Expressions;


namespace Persistence.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PaginatedResult<T>> ToPaginatedResultListAsync<T>(this IQueryable<T> query, int pageIndex, int limit, string? sortColumn = null)
        {

            var totalCount = await query.CountAsync();

            var collection = query;
            
            if (sortColumn != null)
            {
                collection = query
                   .OrderBy(sortColumn, false);
            }

            if (pageIndex != 0 && limit !=0)
            {
                collection = collection.Skip((pageIndex - 1) * limit)
                    .Take(limit);
            }

            List<T> rows;

            try
            {
                rows = await collection.ToListAsync();
            }
            catch (InvalidOperationException)
            {
                rows = collection.ToList();
            }

            return new PaginatedResult<T>(true, rows, null, totalCount, pageIndex, limit);
        }
        
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty, bool desc)
        {
            var command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}