using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExpenseTracker.Application.Common.Patterns;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ExpenseTracker.Infrastructure.Repositeries;

public class GenericRepository<T>
    : IGenericRepository<T>
    where T : class
{
    private readonly ApplicationDbContext _context;

    protected DbSet<T> DbSet =>
        _context.Set<T>();

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<List<TResult>> GetAllAsync<TResult>(
        QueryOptions<T> options,
        Expression<Func<T, TResult>> selector,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = DbSet;

        query = ApplyOptions(query, options);

        return await query
            .Select(selector)
            .ToListAsync(cancellationToken);
    }


    public async Task<List<TResult>> GetAllAsync<TResult>(
        QueryOptions<T> options,
        IConfigurationProvider mapperConfiguration,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = DbSet;

        query = ApplyOptions(query, options);

        return await query
            .ProjectTo<TResult>(mapperConfiguration)
            .ToListAsync(cancellationToken);
    }


    public async Task<TResult?> GetByAsync<TResult>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TResult>> selector,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(predicate)
            .Select(selector)
            .FirstOrDefaultAsync(cancellationToken);
    }


    public async Task<TResult?> GetByAsync<TResult>(
        Expression<Func<T, bool>> predicate,
        IConfigurationProvider mapperConfiguration,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(predicate)
            .ProjectTo<TResult>(mapperConfiguration)
            .FirstOrDefaultAsync(cancellationToken);
    }


    public async Task<T?> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = DbSet;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query
            .FirstOrDefaultAsync(
                predicate,
                cancellationToken);
    }


    public async Task<bool> AnyAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AnyAsync(predicate, cancellationToken);
    }



    public async Task AddAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(
            entity,
            cancellationToken);
    }


    public void Remove(T entity)
    {
        DbSet.Remove(entity);
    }

    public async Task UpdateAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        await Task.CompletedTask;
    }


    public async Task<int> ExecuteUpdateAsync(
        Expression<Func<T, bool>> predicate,
        Action<UpdateSettersBuilder<T>> setProperties,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(predicate)
            .ExecuteUpdateAsync(
                setProperties,
                cancellationToken);
    }


    public async Task<int> ExecuteDeleteAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(predicate)
            .ExecuteDeleteAsync(cancellationToken);
    }


    public async Task<int> CountAsync(
        Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default)
    {
        if (filter is null)
        {
            return await DbSet.CountAsync(cancellationToken);
        }

        return await DbSet.CountAsync(
            filter,
            cancellationToken);
    }


    private static IQueryable<T> ApplyOptions(
        IQueryable<T> query,
        QueryOptions<T> options)
    {
        if (options.Filter is not null)
        {
            query = query.Where(options.Filter);
        }

        if (options.OrderBy is not null)
        {
            query = options.Descending
                ? query.OrderByDescending(options.OrderBy)
                : query.OrderBy(options.OrderBy);
        }

        if (options.Skip.HasValue)
        {
            query = query.Skip(options.Skip.Value);
        }

        if (options.Take.HasValue)
        {
            query = query.Take(options.Take.Value);
        }

        if (!options.Tracking)
        {
            query = query.AsNoTracking();
        }

        return query;
    }

    public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        return DbSet.AddRangeAsync(entities, cancellationToken);
    }
}