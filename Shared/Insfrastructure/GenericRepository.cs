
using Microsoft.EntityFrameworkCore;
using Utility.Shared.Domain;
using System.Linq.Expressions;



namespace Utility.Shared.Insfrastructure
{
    public class GenericRepository<Tkey, T> : IGenericRepository<Tkey, T> where T : class
    {
        private readonly DbContext _context;
        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public bool Create(T Entity)
        {
            _context.Add<T>(Entity);
            return Save();
        }

        public bool Delete(T Entity)
        {
            _context.Remove<T>(Entity);
            return Save();
        }

        public bool ExistBy(Expression<Func<T, bool>> expression) =>
            _context.Set<T>().Any(expression);

        public IEnumerable<T> GetAll() =>
             _context.Set<T>().ToList();

        public IEnumerable<T> GetAllBy(Expression<Func<T, bool>> expression) =>
             _context.Set<T>().Where(expression).ToList();

        public IQueryable<T> GetAllByQuery(Expression<Func<T, bool>> expression) =>
             _context.Set<T>().Where(expression);

        public IQueryable<T> GetAllQuery() =>
             _context.Set<T>();

        public T GetById(Tkey id) =>
            _context.Find<T>(id);

        public bool Save() =>
            _context.SaveChanges() >= 0 ? true : false;
    }
}

