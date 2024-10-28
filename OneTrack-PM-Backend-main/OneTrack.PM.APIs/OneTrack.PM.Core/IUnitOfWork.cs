using OneTrack.PM.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace OneTrack.PM.Core
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, new();
        Task<int> Complete();
    }
}
