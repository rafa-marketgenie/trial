using System.Linq.Expressions;

namespace trial.Repositories
{
    public interface IRepository<T, TKey> where T : class
    {
        Task<T?> GetByIdAsync(TKey id);

        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> ExistsAsync(TKey id);
    }

    public interface IRepository<T> : IRepository<T, int> where T : class
    {
    }
}