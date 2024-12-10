using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mohamy.Core;
using Mohamy.RepositoryLayer.Interfaces;

namespace Mohamy.RepositoryLayer.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly ApplicationDbContext Context;

    public BaseRepository(ApplicationDbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // Query Helper
    private IQueryable<T> PrepareQuery(
        Expression<Func<T, bool>>? criteria = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? skip = null, int? take = null,
        bool isNoTracking = false)
    {
        var query = Context.Set<T>().AsQueryable();

        if (include != null)
            query = include(query);

        if (criteria != null)
            query = query.Where(criteria);

        if (orderBy != null)
            query = orderBy(query);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        if (isNoTracking)
            query = query.AsNoTracking();

        return query;
    }

    // Get All
    public IEnumerable<T> GetAll(Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
    {
        return PrepareQuery(include: include, orderBy: orderBy).ToList();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
    {
        return await PrepareQuery(include: include, orderBy: orderBy).ToListAsync();
    }

    // Get By ID
    public T? GetById(string id)
    {
        return Context.Set<T>().Find(id);
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        return await Context.Set<T>().FindAsync(id);
    }

    // Find
    public T? Find(Expression<Func<T, bool>> criteria, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool isNoTracking = false)
    {
        return PrepareQuery(criteria, include, isNoTracking: isNoTracking).FirstOrDefault();
    }

    public async Task<T?> FindAsync(Expression<Func<T, bool>> criteria, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool isNoTracking = false)
    {
        return await PrepareQuery(criteria, include, isNoTracking: isNoTracking).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> FindAllAsync(
        Expression<Func<T, bool>> criteria,
        int? skip = null, int? take = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool isNoTracking = false)
    {
        return PrepareQuery(criteria, include, orderBy, skip, take, isNoTracking).AsEnumerable();
    }

    // Add
    public T Add(T entity)
    {
        Context.Set<T>().Add(entity);
        return entity;
    }

    public async ValueTask<T> AddAsync(T entity)
    {
        await Context.Set<T>().AddAsync(entity);
        return entity;
    }

    // Add Range
    public IEnumerable<T> AddRange(IEnumerable<T> entities)
    {
        Context.Set<T>().AddRange(entities);
        return entities;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        await Context.Set<T>().AddRangeAsync(entities);
        return entities;
    }

    // Update
    public T Update(T entity)
    {
        Context.Update(entity);
        return entity;
    }

    // Delete
    public void Delete(T entity)
    {
        Context.Set<T>().Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        Context.Set<T>().RemoveRange(entities);
    }

    // Count
    public int Count(Expression<Func<T, bool>>? criteria = null)
    {
        return criteria == null ? Context.Set<T>().Count() : Context.Set<T>().Count(criteria);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? criteria = null)
    {
        return criteria == null ? await Context.Set<T>().CountAsync() : await Context.Set<T>().CountAsync(criteria);
    }

    // Existence Check
    public bool IsExist(Expression<Func<T, bool>> criteria)
    {
        return Context.Set<T>().Any(criteria);
    }

    public async Task<bool> IsExistAsync(Expression<Func<T, bool>> criteria)
    {
        return await Context.Set<T>().AnyAsync(criteria);
    }

    // Resource Disposal
    public async ValueTask DisposeAsync()
    {
        await Context.DisposeAsync();
    }
}
