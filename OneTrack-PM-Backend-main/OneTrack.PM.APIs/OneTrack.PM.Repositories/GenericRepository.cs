using OneTrack.PM.Core.Repositories;
using OneTrack.PM.Core.Specifications;
using OneTrack.PM.Entities.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace OneTrack.PM.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        private readonly OneTrackPMContext _dbContext;

        public GenericRepository(OneTrackPMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetById(int id)
            => _dbContext.Set<T>().Find(id);

        public async Task Add(T entity)
            => await _dbContext.Set<T>().AddAsync(entity);

        public void Delete(T entity)
            => _dbContext.Set<T>().Remove(entity);

        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await _dbContext.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec, bool trackChanges)
            => await ApplySpecifications(spec).ToListAsync();

        public async Task<T> GetByIdAsync(int id)
            => await _dbContext.Set<T>().FindAsync(id);

        public async Task<T> GetWithSpecAsync(ISpecification<T> spec, bool trackChanges)
            => await (trackChanges ? ApplySpecifications(spec).AsTracking().FirstOrDefaultAsync() : ApplySpecifications(spec).AsNoTracking().FirstOrDefaultAsync());

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
            => await ApplySpecifications(spec).CountAsync();

        public bool IsExistWithSpec(ISpecification<T> spec)
            => ApplySpecifications(spec).Any();

        public void Update(T entity)
            => _dbContext.Set<T>().Update(entity);

        private IQueryable<T> ApplySpecifications(ISpecification<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        public void DeleteAll(IEnumerable<T> entities)
            => _dbContext.Set<T>().RemoveRange(entities);
    }
}
