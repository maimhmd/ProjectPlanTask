using OneTrack.PM.Core.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneTrack.PM.Core.Repositories
{
    public interface IGenericRepository<T> where T : class, new()
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        T GetById(int id);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec, bool trackChanges);
        Task<T> GetWithSpecAsync(ISpecification<T> spec, bool trackChanges);
        Task<int> GetCountWithSpecAsync(ISpecification<T> spec);
        bool IsExistWithSpec(ISpecification<T> spec);
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteAll(IEnumerable<T> entities);
    }
}
