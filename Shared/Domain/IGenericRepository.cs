using System.Linq.Expressions;
using Utility.Shared.Application;
namespace Utility.Shared.Domain
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : class
    {
        IQueryable<TEntity> GetAllAsync();
        IQueryable<TEntity> GetAllBy(Expression<Func<TEntity, bool>> expression);
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<OperationResult> CreateAsync(TEntity entity);
        Task<OperationResult> DeleteAsync(TEntity entity);
        Task<bool> ExistByAsync(Expression<Func<TEntity, bool>> expression);
        Task<bool> SaveAsync();
    }

}
