
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Shared.Domain;
using Shared.Application;



namespace Shared.Insfrastructure
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class
    {
        private readonly DbContext _context;
        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult> CreateAsync(TEntity entity)
        {
            try
            {
                _context.Add(entity);
                await SaveAsync();
                return new OperationResult(true);
            }
            catch (Exception)
            {

                return new OperationResult(false);
            }
        }

        public async Task<OperationResult> DeleteAsync(TEntity entity)
        {

            try
            {
                _context.Remove(entity);
                await SaveAsync();
                return new OperationResult(true);
            }
            catch (Exception)
            {

                return new OperationResult(false);
            }
        }

        public async Task<bool> ExistByAsync(Expression<Func<TEntity, bool>> expression) =>
                    await _context.Set<TEntity>().AsNoTracking().AnyAsync(expression);

        public IQueryable<TEntity> GetAll() =>
                        _context.Set<TEntity>().AsNoTracking();

        public IQueryable<TEntity> GetAllBy(Expression<Func<TEntity, bool>> expression) =>
                         _context.Set<TEntity>().Where(expression).AsNoTracking();

        public async Task<TEntity?> GetByIdAsync(TKey id) =>
                    await _context.FindAsync<TEntity>(id);

        public async Task<bool> SaveAsync() =>
                    await _context.SaveChangesAsync() > 0 ? true : false;

    }
}

