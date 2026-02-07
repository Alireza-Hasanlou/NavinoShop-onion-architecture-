using Shared.Application;
using System.Linq.Expressions;
namespace Shared.Domain
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAllBy(Expression<Func<TEntity, bool>> expression);
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<OperationResult> CreateAsync(TEntity entity);
        Task<OperationResult> DeleteAsync(TEntity entity);
        Task<bool> ExistByAsync(Expression<Func<TEntity, bool>> expression);
        Task<bool> SaveAsync();
    }

}
