using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Mohamy.RepositoryLayer.Interfaces;

public interface IBaseRepository<T> where T : class
{
    // Retrieve all entities
    IEnumerable<T> GetAll(Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
    Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

    // Retrieve a single entity by ID
    T? GetById(string id);
    Task<T?> GetByIdAsync(string id);

    // Find an entity
    T? Find(Expression<Func<T, bool>> criteria, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool isNoTracking = false);
    Task<T?> FindAsync(Expression<Func<T, bool>> criteria, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool isNoTracking = false);

    // Retrieve all entities matching a condition
    Task<IEnumerable<T>> FindAllAsync(
        Expression<Func<T, bool>> criteria,
        int? skip = null, int? take = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool isNoTracking = false);

    // Add entities
    T Add(T entity);
    ValueTask<T> AddAsync(T entity);
    IEnumerable<T> AddRange(IEnumerable<T> entities);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

    // Update entity
    T Update(T entity);

    // Delete entities
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);

    // Count entities
    int Count(Expression<Func<T, bool>>? criteria = null);
    Task<int> CountAsync(Expression<Func<T, bool>>? criteria = null);

    // Check existence
    bool IsExist(Expression<Func<T, bool>> criteria);
    Task<bool> IsExistAsync(Expression<Func<T, bool>> criteria);
}
