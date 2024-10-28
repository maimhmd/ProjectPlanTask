using OneTrack.PM.Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace OneTrack.PM.Repositories
{
    public static class SpecificationEvalutor<TEntity> where TEntity : class, new()
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;
            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);
            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            if (spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);
            if (spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);
            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression)
                => currentQuery.Include(includeExpression));
            query = spec.IncludeStrings
             .Aggregate(query,
                 (current, include) => current.Include(include));
            return query;
        }
    }
}
