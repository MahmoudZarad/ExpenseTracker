using ExpenseTracker.Application.Common.Patterns;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;


namespace ExpenseTracker.Application.Interfaces.Repositories;

public interface IGenericRepository<T>
    where T : class
{
    Task<List<TResult>> GetAllAsync<TResult>(
        QueryOptions<T> options,
        Expression<Func<T, TResult>> selector,
        CancellationToken cancellationToken = default);

    Task<List<TResult>> GetAllAsync<TResult>(
        QueryOptions<T> options,
        IConfigurationProvider mapperConfiguration,
        CancellationToken cancellationToken = default);

    Task<TResult?> GetByAsync<TResult>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TResult>> selector,
        CancellationToken cancellationToken = default);

    Task<TResult?> GetByAsync<TResult>(
        Expression<Func<T, bool>> predicate,
        IConfigurationProvider mapperConfiguration,
        CancellationToken cancellationToken = default);

    Task<T?> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes);

    Task<bool> AnyAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        T entity,
        CancellationToken cancellationToken = default);

    Task AddRangeAsync(
    IEnumerable<T> entities,
    CancellationToken cancellationToken = default);

    void Remove(T entity);

    Task UpdateAsync(
        T entity,
        CancellationToken cancellationToken = default);

    Task<int> ExecuteUpdateAsync(
        Expression<Func<T, bool>> predicate,
        Action<UpdateSettersBuilder<T>> setProperties,
        CancellationToken cancellationToken = default);

    Task<int> ExecuteDeleteAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default);
}