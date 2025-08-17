using System.Linq.Expressions;
using Utility.Shared.Application;
namespace Utility.Shared.Domain
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<OperationResult> CreateAsync(TEntity entity);
        Task<OperationResult> UpdateAsync(TEntity entity);
        Task<OperationResult> DeleteAsync(TEntity entity);
        Task<bool> ExistByAsync(Expression<Func<TEntity, bool>> expression);
        Task SaveAsync();
    }

}
