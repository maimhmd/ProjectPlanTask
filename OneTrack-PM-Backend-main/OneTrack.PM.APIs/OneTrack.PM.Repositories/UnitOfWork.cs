using OneTrack.PM.Core;
using OneTrack.PM.Core.Repositories;
using OneTrack.PM.Entities.Models.DB;
using System.Collections;
using System.Threading.Tasks;

namespace OneTrack.PM.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OneTrackPMContext _dbContext;
        private Hashtable _repositories;
        public UnitOfWork(OneTrackPMContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class,new()    
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(type, repository);
            }
            return _repositories[type] as IGenericRepository<TEntity>;
        }

        public async Task<int> Complete()
            => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();
    }
}