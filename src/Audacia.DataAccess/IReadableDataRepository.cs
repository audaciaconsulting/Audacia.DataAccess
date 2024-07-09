using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Audacia.Core;
using Audacia.DataAccess.Specifications;
using Audacia.DataAccess.Specifications.DataStoreImplementations;
using Audacia.DataAccess.Specifications.Ordering;
using Audacia.DataAccess.Specifications.Paging;
using Audacia.DataAccess.Specifications.Paging.Sorting;
using Audacia.DataAccess.Specifications.Projection;

namespace Audacia.DataAccess;

/// <summary>
/// Exposes the methods to allow the underlying data storage to be queried.
/// The query rules are encapsulated in the specification objects passed into each method as a parameter.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "AV1554:Method contains optional parameter in type hierarchy", Justification = "Allows to include an existing cancellation token when invoking methods. Otherwise, a new token is provided.")]
public interface IReadableDataRepository
{
    /// <summary>
    /// Asynchronously determines whether all the elements of type <typeparamref name="T"/> satisfy the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <param name="specification">Query rules.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>If all elements in a query match the defined rules.</returns>
    Task<bool> AllAsync<T>(
        IQuerySpecification<T> specification,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Asynchronously determines whether any elements of type <typeparamref name="T"/> satisfy the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <param name="specification">Query rules.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>If at any, at least one element, in a query match the defined rules.</returns>
    Task<bool> AnyAsync<T>(
        IQuerySpecification<T> specification,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Asynchronously gets the count of elements of type <typeparamref name="T"/> satisfy the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <param name="specification">Query rules.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>The number of results in a query that match the defined rules.</returns>
    Task<int> GetCountAsync<T>(
        IQuerySpecification<T> specification,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Gets the first instance of the model of type <typeparamref name="T"/> that matches the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <param name="specification">Query rules.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>If at any, at least one element, in a query that match the defined rules.</returns>
    Task<T?> GetAsync<T>(
        IOrderableQuerySpecification<T> specification,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Gets all instances of the model of type <typeparamref name="T"/> that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <param name="specification">Query rules.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>All the elements in a query that match the defined rules.</returns>
    Task<IEnumerable<T>> GetAllAsync<T>(
        IOrderableQuerySpecification<T> specification,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Gets the first instance of the model of type <typeparamref name="T"/>, projected to an object of type <typeparamref name="TResult"/>,
    /// that matches the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <typeparam name="TResult">Elemety type of the returned collection.</typeparam>
    /// <param name="specification">Query rules.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>The first element in a query that matches the defined rules.</returns>
    Task<TResult?> GetAsync<T, TResult>(
        IProjectableQuerySpecification<T, TResult> specification,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Gets all instances of the model of type <typeparamref name="T"/>, projected to an object of type <typeparamref name="TResult"/>,
    /// that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <typeparam name="TResult">Elemety type of the returned collection.</typeparam>
    /// <param name="specification">Query rules.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>All the elements in a query that match the defined rules.</returns>
    Task<IEnumerable<TResult>> GetAllAsync<T, TResult>(
        IProjectableQuerySpecification<T, TResult> specification,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Gets the first instance of the model of type <typeparamref name="T"/>, projected to an object of type <typeparamref name="TResult"/>,
    /// that matches the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <typeparam name="TResult">Elemety type of the returned collection.</typeparam>
    /// <param name="specification">Query rules.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>The first item in an ordered list of elements from a query that match the defined rules.</returns>
    Task<TResult?> GetAsync<T, TResult>(
        IOrderableQuerySpecification<T, TResult> specification,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Gets all instances of the model of type <typeparamref name="T"/>, projected to an object of type <typeparamref name="TResult"/>,
    /// that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <typeparam name="TResult">Elemety type of the returned collection.</typeparam>
    /// <param name="specification">Query rules.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>An ordered collection of elements from a query that match the defined rules.</returns>
    Task<IEnumerable<TResult>> GetAllAsync<T, TResult>(
        IOrderableQuerySpecification<T, TResult> specification,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Gets a page of the model of type <typeparamref name="T"/> that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <param name="specification">Query rules.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>A paged collection of elements from a query that match the defined rules.</returns>
    Task<IPage<T>> GetPageAsync<T>(
        IPageableQuerySpecification<T> specification,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Gets a page of the model of type <typeparamref name="T"/>, projected to an object of type <typeparamref name="TResult"/>, that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <typeparam name="TResult">Elemety type of the returned collection.</typeparam>
    /// <param name="specification">Query rules.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>A paged collection of elements from a query that match the defined rules.</returns>
    Task<IPage<TResult>> GetPageAsync<T, TResult>(
        IPageableQuerySpecification<T, TResult> specification,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Gets a page of the model of type <typeparamref name="T"/> that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <param name="specification">Query rules.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>A sorted paged collection of elements from a query that match the defined rules.</returns>
    Task<IPage<T>> GetPageAsync<T>(
        ISortablePageableQuerySpecification<T> specification,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Gets a page of the model of type <typeparamref name="T"/>, projected to an object of type <typeparamref name="TResult"/>, that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <typeparam name="TResult">Elemety type of the returned collection.</typeparam>
    /// <param name="specification">Query rules.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>A sorted paged collection of elements from a query that match the defined rules.</returns>
    Task<IPage<TResult>> GetPageAsync<T, TResult>(
        ISortablePageableQuerySpecification<T, TResult> specification,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Gets all instances of the model of type <typeparamref name="T"/> that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <param name="specification">Query rules.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>An collection of elements from a query that match the defined rules.</returns>
    Task<IEnumerable<T>> GetAllAsync<T>(
        IDataStoreImplementedQuerySpecification<T> specification,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Gets all instances of the model of type <typeparamref name="T"/>, projected to an object of type <typeparamref name="TResult"/>,
    /// that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <typeparam name="TResult">Elemety type of the returned collection.</typeparam>
    /// <param name="specification">Query rules.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>An collection of elements from a query that match the defined rules.</returns>
    Task<IEnumerable<TResult>> GetAllAsync<T, TResult>(
        IDataStoreImplementedQuerySpecification<T, TResult> specification,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Gets a DisposableSwitch which implements IDisposable, used to define the scope for which executed queries
    /// will track changes on the entities which are returned.
    /// </summary>
    /// <returns>A disposable switch for tracking changes on entities.</returns>
    DisposableSwitch BeginTrackChanges();
}