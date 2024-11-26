using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Audacia.Core.Extensions;
using Audacia.Core;
using Audacia.DataAccess.Specifications;
using Audacia.DataAccess.Specifications.DataStoreImplementations;
using Audacia.DataAccess.Specifications.Including;
using Audacia.DataAccess.Specifications.Ordering;
using Audacia.DataAccess.Specifications.Paging;
using Audacia.DataAccess.Specifications.Paging.Sorting;
using Audacia.DataAccess.Specifications.Projection;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.SqlServer;

/// <summary>
/// Implements <see cref="IReadableDataRepository" />.
/// </summary>
/// <typeparam name="TContext">Type of <see cref="ReadDataRepository{TContext}"/>. </typeparam>
public class ReadDataRepository<TContext> : IReadableDataRepository, IDisposable
    where TContext : DbContext
{
    private readonly TContext _context;
    private readonly StoredProcedureBuilder _storedProcedureBuilder;
    private bool _trackChanges;
    private bool _disposed;

    /// <summary>
    /// Constructor takes in an instance of the Data Context and StoredProcedureBuilder.
    /// </summary>
    /// <param name="context">Instance of TContext.</param>
    /// <param name="storedProcedureBuilder">Instance of <see cref="StoredProcedureBuilder"/>.</param>
    public ReadDataRepository(TContext context, StoredProcedureBuilder storedProcedureBuilder)
    {
        _context = context;
        _storedProcedureBuilder = storedProcedureBuilder;
    }

    /// <summary>
    /// Track changes of the Data Context.
    /// </summary>
    /// <returns><see cref="DisposableSwitch"/>.</returns>
    public DisposableSwitch BeginTrackChanges()
    {
        return new DisposableSwitch(() => _trackChanges = true, () => _trackChanges = false);
    }

    /// <summary>
    /// Asynchronously determines whether all the elements of type <typeparamref name="T"/> satisfy the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IQuerySpecification{T}"/>.</typeparam>
    /// <param name="specification">Instance of <see cref="IQuerySpecification{T}"/>.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>If all elements in a query match the defined rules.</returns>
    public async Task<bool> AllAsync<T>(
        IQuerySpecification<T> specification,
        CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification, nameof(specification));
        if (specification.Filter == null)
        {
            // no filter has been provided
            return false;
        }

        var query = ApplyIncludes(specification).AsNoTracking();

        return await query.AllAsync(specification.Filter.Expression, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously determines whether any elements of type <typeparamref name="T"/> satisfy the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IQuerySpecification{T}"/>.</typeparam>
    /// <param name="specification">Instance of <see cref="IQuerySpecification{T}"/>.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>If at any, at least one element, in a query match the defined rules.</returns>
    public async Task<bool> AnyAsync<T>(
        IQuerySpecification<T> specification,
        CancellationToken cancellationToken = default) where T : class
    {
        var query = ApplyIncludesAndFilter(specification).AsNoTracking();

        return await query.AnyAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously gets the count of elements of type <typeparamref name="T"/> satisfy the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <param name="specification">Instance of <see cref="IQuerySpecification{T}"/>.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>The number of results in a query that match the defined rules.</returns>
    public async Task<int> GetCountAsync<T>(
        IQuerySpecification<T> specification,
        CancellationToken cancellationToken = default) where T : class
    {
        var query = ApplyIncludesAndFilter(specification);

        return await query.CountAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the first instance of the model of type <typeparamref name="T"/> that matches the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <param name="specification">Instance of <see cref="IQuerySpecification{T}"/>.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>If at any, at least one element, in a query that match the defined rules.</returns>
    public async Task<T?> GetAsync<T>(
        IOrderableQuerySpecification<T> specification,
        CancellationToken cancellationToken = default) where T : class
    {
        var query = ApplyIncludesAndFilter(specification);

        if (specification.Order != null)
        {
            query = PerformOrdering(specification.Order, query);
        }

        ArgumentNullException.ThrowIfNull(query, nameof(query));
        if (!_trackChanges)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets all instances of the model of type <typeparamref name="T"/> that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <param name="specification">Instance of <see cref="IQuerySpecification{T}"/>.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>All the elements in a query that match the defined rules.</returns>
    public async Task<IEnumerable<T>> GetAllAsync<T>(
        IOrderableQuerySpecification<T> specification,
        CancellationToken cancellationToken = default) where T : class
    {
        var query = ApplyIncludesAndFilter(specification);

        if (specification.Order?.OrderSteps.Count() > 0)
        {
            query = PerformOrdering(specification.Order, query);
        }

        ArgumentNullException.ThrowIfNull(query, nameof(query));
        if (!_trackChanges)
        {
            query = query.AsNoTracking();
        }

        return await query.ToArrayAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the first instance of the model of type <typeparamref name="T"/>, projected to an object of type <typeparamref name="TResult"/>,
    /// that matches the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <typeparam name="TResult">Elemety type of the returned collection.</typeparam>
    /// <param name="specification">Instance of <see cref="IProjectableQuerySpecification{T,TResult}"/>.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <exception cref = "ArgumentNullException" >Throws ArgumentNullException when IProjectableQuerySpecification.Projection is null.</exception >
    /// <returns>The first element in a query that matches the defined rules.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Null check is done on a property of the parameter object.")]
    public async Task<TResult?> GetAsync<T, TResult>(
        IProjectableQuerySpecification<T, TResult> specification,
        CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification, nameof(specification));
        if (specification.Projection == null)
        {
            throw new ArgumentNullException(nameof(specification.Projection), "Cannot project with no projection specification");
        }

        var query = ApplyIncludesAndFilter(specification);

        if (!_trackChanges)
        {
            query = query.AsNoTracking();
        }

        var projectedQuery = query.Select(specification.Projection.Expression);

        return await projectedQuery.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets all instances of the model of type <typeparamref name="T"/>, projected to an object of type <typeparamref name="TResult"/>,
    /// that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <typeparam name="TResult">Elemety type of the returned collection.</typeparam>
    /// <param name="specification">Instance of <see cref="IProjectableQuerySpecification{T,TResult}"/>.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <exception cref = "ArgumentNullException" >Throws ArgumentNullException when IProjectableQuerySpecification.Projection is null.</exception >
    /// <returns>All the elements in a query that match the defined rules.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Null check is done on a property of the parameter object.")]
    public async Task<IEnumerable<TResult>> GetAllAsync<T, TResult>(
        IProjectableQuerySpecification<T, TResult> specification,
        CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification, nameof(specification));

        if (specification.Projection == null)
        {
            throw new ArgumentNullException(nameof(specification.Projection), "Cannot project with no projection specification");
        }

        var query = ApplyIncludesAndFilter(specification);

        if (!_trackChanges)
        {
            query = query.AsNoTracking();
        }

        var projectedQuery = query.Select(specification.Projection.Expression);

        return await projectedQuery.ToArrayAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the first instance of the model of type <typeparamref name="T"/>, projected to an object of type <typeparamref name="TResult"/>,
    /// that matches the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <typeparam name="TResult">Elemety type of the returned collection.</typeparam>
    /// <param name="specification">Instance of <see cref="IOrderableQuerySpecification{T,TResult}"/>.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <exception cref = "ArgumentNullException" >Throws ArgumentNullException when IProjectableQuerySpecification.Projection is null.</exception >
    /// <returns>The first item in an ordered list of elements from a query that match the defined rules.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Null check is done on a property of the parameter object.")]
    public async Task<TResult?> GetAsync<T, TResult>(
        IOrderableQuerySpecification<T, TResult> specification,
        CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification, nameof(specification));

        if (specification.Projection == null)
        {
            throw new ArgumentNullException(nameof(specification.Projection), "Cannot project with no projection specification");
        }

        var query = ApplyIncludesAndFilter(specification);

        if (!_trackChanges)
        {
            query = query.AsNoTracking();
        }

        var projectedQuery = query.Select(specification.Projection.Expression);

        if (specification.Order != null)
        {
            projectedQuery = PerformOrdering(specification.Order, projectedQuery);
        }

        ArgumentNullException.ThrowIfNull(projectedQuery, nameof(projectedQuery));

        return await projectedQuery.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets all instances of the model of type <typeparamref name="T"/>, projected to an object of type <typeparamref name="TResult"/>,
    /// that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <typeparam name="TResult">Elemety type of the returned collection.</typeparam>
    /// <param name="specification">Instance of <see cref="IOrderableQuerySpecification{T,TResult}"/>.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <exception cref = "ArgumentNullException" >Throws ArgumentNullException when IProjectableQuerySpecification.Projection is null.</exception >
    /// <returns>An ordered collection of elements from a query that match the defined rules.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Null check is done on a property of the parameter object.")]
    public async Task<IEnumerable<TResult>> GetAllAsync<T, TResult>(
        IOrderableQuerySpecification<T, TResult> specification,
        CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification, nameof(specification));
        if (specification.Projection == null)
        {
            throw new ArgumentNullException(nameof(specification.Projection), "Cannot project with no projection specification");
        }

        var query = ApplyIncludesAndFilter(specification);

        if (!_trackChanges)
        {
            query = query.AsNoTracking();
        }

        var projectedQuery = query.Select(specification.Projection.Expression);

        if (specification.Order != null)
        {
            projectedQuery = PerformOrdering(specification.Order, projectedQuery);
        }

        ArgumentNullException.ThrowIfNull(projectedQuery, nameof(projectedQuery));
        return await projectedQuery.ToArrayAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a page of the model of type <typeparamref name="T"/> that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <param name="specification">Instance of <see cref="IPageableQuerySpecification{T}"/>.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <exception cref = "ArgumentNullException" >Throws ArgumentNullException when IProjectableQuerySpecification.Projection is null.</exception >
    /// <returns>A paged collection of elements from a query that match the defined rules.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Null check is done on a property of the parameter object.")]
    public Task<IPage<T>> GetPageAsync<T>(
        IPageableQuerySpecification<T> specification,
        CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification, nameof(specification));
        if (specification.Order == null)
        {
            throw new ArgumentNullException(nameof(specification.Order), "Cannot page query with no order specification");
        }

        var query = ApplyIncludesAndFilter(specification);

        if (!_trackChanges)
        {
            query = query.AsNoTracking();
        }

        query = PerformOrdering(specification.Order, query);

        ArgumentNullException.ThrowIfNull(query, nameof(query));

        return Task.FromResult<IPage<T>>(new Page<T>(query, specification.PagingRequest));
    }

    /// <summary>
    /// Gets a page of the model of type <typeparamref name="T"/>, projected to an object of type <typeparamref name="TResult"/>, that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <typeparam name="TResult">Elemety type of the returned collection.</typeparam>
    /// <param name="specification">Instance of <see cref="IPageableQuerySpecification{T}"/>.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <exception cref = "ArgumentNullException" >Throws ArgumentNullException when IProjectableQuerySpecification.Projection is null.</exception >
    /// <returns>A paged collection of elements from a query that match the defined rules.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Null check is done on a property of the parameter object.")]
    public Task<IPage<TResult>> GetPageAsync<T, TResult>(
        IPageableQuerySpecification<T, TResult> specification,
        CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification, nameof(specification));
        if (specification.Projection == null)
        {
            throw new ArgumentNullException(nameof(specification.Projection), "Cannot project with no projection specification");
        }

        if (specification.Order == null)
        {
            throw new ArgumentNullException(nameof(specification.Order), "Cannot page query with no order specification");
        }

        var query = ApplyIncludesAndFilter(specification);

        if (!_trackChanges)
        {
            query = query.AsNoTracking();
        }

        var projectedQuery = query.Select(specification.Projection.Expression);

        if (specification.Order != null)
        {
            projectedQuery = PerformOrdering(specification.Order, projectedQuery);
        }

        ArgumentNullException.ThrowIfNull(projectedQuery, nameof(projectedQuery));

        return Task.FromResult<IPage<TResult>>(new Page<TResult>(projectedQuery, specification.PagingRequest));
    }

    /// <summary>
    /// Gets a page of the model of type <typeparamref name="T"/> that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <param name="specification">Instance of <see cref="ISortablePageableQuerySpecification{T}"/>.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>A sorted paged collection of elements from a query that match the defined rules.</returns>
    public Task<IPage<T>> GetPageAsync<T>(
        ISortablePageableQuerySpecification<T> specification,
        CancellationToken cancellationToken = default(CancellationToken)) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification, nameof(specification));
        var query = ApplyIncludesAndFilter(specification);

        if (!_trackChanges)
        {
            query = query.AsNoTracking();
        }

        return Task.FromResult<IPage<T>>(new Page<T>(query, specification.SortablePagingRequest));
    }

    /// <summary>
    /// Gets a page of the model of type <typeparamref name="T"/>, projected to an object of type <typeparamref name="TResult"/>, that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <typeparam name="TResult">Elemety type of the returned collection.</typeparam>
    /// <param name="specification">Instance of <see cref="ISortablePageableQuerySpecification{T}"/>.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <exception cref = "ArgumentNullException" >Throws ArgumentNullException when IProjectableQuerySpecification.Projection is null.</exception >
    /// <returns>A sorted paged collection of elements from a query that match the defined rules.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Null check is done on a property of the parameter object.")]
    public Task<IPage<TResult>> GetPageAsync<T, TResult>(
        ISortablePageableQuerySpecification<T, TResult> specification,
        CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification, nameof(specification));
        if (specification.Projection == null)
        {
            throw new ArgumentNullException(nameof(specification.Projection), "Cannot project with no projection specification");
        }

        var query = ApplyIncludesAndFilter(specification);

        if (!_trackChanges)
        {
            query = query.AsNoTracking();
        }

        var projectedQuery = query.Select(specification.Projection.Expression);

        return Task.FromResult<IPage<TResult>>(new Page<TResult>(
            projectedQuery,
            specification.SortablePagingRequest));
    }

    /// <summary>
    /// Gets all instances of the model of type <typeparamref name="T"/> that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <param name="specification">Instance of <see cref="IDataStoreImplementedQuerySpecification{T}"/>.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>An collection of elements from a query that match the defined rules.</returns>
    public async Task<IEnumerable<T>> GetAllAsync<T>(
        IDataStoreImplementedQuerySpecification<T> specification,
        CancellationToken cancellationToken = default) where T : class
    {
        var commandText = _storedProcedureBuilder.GetQueryText(specification);

        var query = _context.Set<T>()
            .FromSqlRaw(commandText);

        if (!_trackChanges)
        {
            query = query.AsNoTracking();
        }

        return await query.ToArrayAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets all instances of the model of type <typeparamref name="T"/>, projected to an object of type <typeparamref name="TResult"/>,
    /// that match the rules in the given <paramref name="specification"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <typeparam name="TResult">Elemety type of the returned collection.</typeparam>
    /// <param name="specification">Instance of <see cref="IDataStoreImplementedQuerySpecification{T}"/>.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>An collection of elements from a query that match the defined rules.</returns>
    public async Task<IEnumerable<TResult>> GetAllAsync<T, TResult>(
        IDataStoreImplementedQuerySpecification<T, TResult> specification,
        CancellationToken cancellationToken = default) where T : class
    {
        var commandText = _storedProcedureBuilder.GetQueryText(specification);

        var query = _context.Set<T>()
            .FromSqlRaw(commandText);

        if (!_trackChanges)
        {
            query = query.AsNoTracking();
        }

        return await query.Select(specification.Projection.Expression).ToArrayAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Apply includes from <see cref="IQuerySpecification{T}"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <param name="specification">Instance of <see cref="IQuerySpecification{T}"/>.</param>
    /// <returns>An collection of elements from a query that match the defined rules.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Member-design", "AV1130:Return interfaces to unchangeable collections", Justification = "Method is designed to return a Linq Query and changing the design will introduce breaking changes.")]
    public IQueryable<T> ApplyIncludes<T>(IQuerySpecification<T> specification) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification, nameof(specification));

        var query = _context.Set<T>().AsQueryable();

        if (specification.Include != null)
        {
            query = ApplyIncludes(specification.Include, query);
        }

        return query;
    }

    /// <summary>
    /// Apply Includes and Filters from <see cref="IQuerySpecification{T}"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <param name="specification">Instance of <see cref="IQuerySpecification{T}"/>.</param>
    /// <returns>An collection of elements from a query that match the defined rules.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Member-design", "AV1115:A property, method or local function should do only one thing", Justification = "Changing the method name will introduce breaking changes.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Member-design", "AV1130:Return interfaces to unchangeable collections", Justification = "Method is designed to return a Linq Query and changing the design will introduce breaking changes.")]
    public IQueryable<T> ApplyIncludesAndFilter<T>(IQuerySpecification<T> specification) where T : class
    {
        ArgumentNullException.ThrowIfNull(specification, nameof(specification));

        var query = _context.Set<T>().AsQueryable();

        if (specification.Include != null)
        {
            query = ApplyIncludes(specification.Include, query);
        }

        if (specification.Filter != null)
        {
            query = query.Where(specification.Filter.Expression);
        }

        return query;
    }

    private static IQueryable<T> ApplyIncludes<T>(
        IIncludeSpecification<T> includeSpecification,
        IQueryable<T> query)
        where T : class
    {
        foreach (var path in includeSpecification.IncludeStepPaths)
        {
            var parts = new List<string>();
            query = AddParts(parts, path, query);
        }

        return query;
    }

    private static IQueryable<T> AddParts<T>(List<string> parts, IncludeStepPath? path, IQueryable<T> query) where T : class
    {
        ArgumentNullException.ThrowIfNull(path, nameof(path));

        foreach (var step in path)
        {
            var propertyInfo = ExpressionExtensions.GetPropertyInfo(step.Expression);

            if (propertyInfo != null)
            {
                parts.Add(propertyInfo.Name);

                var strPath = string.Join(".", parts);

                query = query.Include(strPath);
            }
        }

        return query;
    }

    /// <summary>
    /// Add order using <see cref="IOrderSpecification{T}"/>.
    /// </summary>
    /// <typeparam name="T">Elemety type of the query.</typeparam>
    /// <param name="orderSpecification">Instance of <see cref="IOrderSpecification{T}"/>.</param>
    /// <param name="query">Query which the ordering will be applied to.</param>
    /// <returns>An collection of elements from a query that match the defined rules.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Member-design", "AV1130:Return interfaces to unchangeable collections", Justification = "Method is designed to return a Linq Query and changing the design will introduce breaking changes.")]
    public IOrderedQueryable<T>? PerformOrdering<T>(
        IOrderSpecification<T> orderSpecification,
        IQueryable<T> query)
    {
        ArgumentNullException.ThrowIfNull(orderSpecification, nameof(orderSpecification));
        var steps = new Queue<OrderStep>(orderSpecification.OrderSteps);

        var orderedQuery = PerformOrderStep(query, steps.Dequeue());

        while (steps.Any())
        {
            if (orderedQuery != null)
            {
                orderedQuery = PerformOrderStep(orderedQuery, steps.Dequeue());
            }
        }

        return orderedQuery;
    }

    private IOrderedQueryable<T>? PerformOrderStep<T>(IOrderedQueryable<T> query, OrderStep orderStep)
    {
        var orderMethod = orderStep.Ascending ? nameof(Queryable.ThenBy) : nameof(Queryable.ThenByDescending);

        return PerformOrderStep(query, orderStep, orderMethod);
    }

    private IOrderedQueryable<T>? PerformOrderStep<T>(IQueryable<T> query, OrderStep orderStep)
    {
        var orderMethod = orderStep.Ascending ? nameof(Queryable.OrderBy) : nameof(Queryable.OrderByDescending);

        return PerformOrderStep(query, orderStep, orderMethod);
    }

    private static IOrderedQueryable<T>? PerformOrderStep<T>(IQueryable<T> query, OrderStep orderStep,
        string orderMethod)
    {
        var method =
            typeof(Queryable).GetMethods().First(m => m.Name == orderMethod && m.GetParameters().Length == 2);

        var genericMethod = method.MakeGenericMethod(typeof(T), orderStep.Type);

        ArgumentNullException.ThrowIfNull(genericMethod);

        return genericMethod.Invoke(null, new object[] { query, orderStep.Expression }) as IOrderedQueryable<T>;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose <typeparamref name="TContext"/> object.
    /// </summary>
    /// <param name="disposing">Whether we're disposing or not.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP007:Don't dispose injected", Justification = "Needs this for clearing out context object.")]
    protected virtual void Dispose(bool disposing)
    {
        // Check to see if Dispose has already been called.
        if (!_disposed)
        {
            // If disposing equals true, dispose all managed
            // and unmanaged resources.
            if (disposing)
            {
                // Dispose managed resources.
                _context.Dispose();
            }

            _disposed = true;
        }
    }
}