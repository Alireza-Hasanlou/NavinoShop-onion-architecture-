
using Microsoft.EntityFrameworkCore;
using Utility.Shared.Domain;
using System.Linq.Expressions;
using Utility.Shared.Application;



namespace Utility.Shared.Insfrastructure
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
            catch (Exception ex)
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
                return new OperationResult(true, "Deleted");
            }
            catch (Exception ex)
            {

                return new OperationResult(false, "An error occurred while Deleting entity");
            }
        }

        public async Task<bool> ExistByAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>().AsNoTracking().AnyAsync(expression);

        }


        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>().Where(expression).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _context.FindAsync<TEntity>(id);
        }

        public async Task<OperationResult> UpdateAsync(TEntity entity)
        {
            try
            {
                _context.Update(entity);
                await SaveAsync();
                return new OperationResult(true, "Updated");
            }
            catch (Exception)
            {

                return new OperationResult(false, "An error occurred while Updating entity");
            }

        }

       public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

