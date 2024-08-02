using System;
using Audacia.Core;
using Audacia.DataAccess.Specifications.Filtering;
using Audacia.DataAccess.Specifications.Including;
using Audacia.DataAccess.Specifications.Ordering;
using Audacia.DataAccess.Specifications.Projection;

namespace Audacia.DataAccess.Specifications.Paging;

/// <summary>
/// Default implementation of <see cref="IPageableQuerySpecification{T}"/> that
/// allows a collection of objects of type <see cref="IPageableQuerySpecification{T}"/> to be filtered, paged
/// and have navigation properties included in results.
/// </summary>
/// <typeparam name="T">Type of <see cref="IPageableQuerySpecification{T}"/>.</typeparam>
public class PageableQuerySpecification<T> : IPageableQuerySpecification<T> where T : class
{
    /// <summary>
    /// Gets or sets Filter.
    /// </summary>
    public IFilterSpecification<T>? Filter { get; set; }

    /// <summary>
    /// Gets or sets Include.
    /// </summary>
    public IIncludeSpecification<T>? Include { get; set; }

    /// <summary>
    /// Gets or sets Order.
    /// </summary>
    public IOrderSpecification<T>? Order { get; set; }

    /// <summary>
    /// Gets or sets PagingRequest.
    /// </summary>
    public PagingRequest PagingRequest { get; set; }

    /// <summary>
    /// Constructor takes <see cref="IOrderableQuerySpecification{T}"/> BuildForm and PagingRequest.
    /// </summary>
    /// <param name="buildFrom">Instance of <see cref="IOrderableQuerySpecification{T}"/>.</param>
    /// <param name="sortablePagingRequest">Instance of PagingRequest.</param>
    public PageableQuerySpecification(
        IOrderableQuerySpecification<T> buildFrom,
        PagingRequest sortablePagingRequest)
    {
        ArgumentNullException.ThrowIfNull(buildFrom, nameof(buildFrom));
        ArgumentNullException.ThrowIfNull(sortablePagingRequest, nameof(sortablePagingRequest));

        Filter = buildFrom.Filter;
        Include = buildFrom.Include;
        Order = buildFrom.Order;

        PagingRequest = sortablePagingRequest;
    }
}

/// <summary>
/// Default implementation of <see cref="IPageableQuerySpecification{T,TResult}"/> that
/// allows a collection of objects of type <see cref="IPageableQuerySpecification{T}"/> to be filtered, paged, converted to objects
/// of type <see cref="IPageableQuerySpecification{TResult}"/> and have navigation properties included in results .
/// </summary>
/// <typeparam name="T">Type of <see cref="IPageableQuerySpecification{T}"/>.</typeparam>
/// <typeparam name="TResult">Return type of <see cref="IPageableQuerySpecification{TResult}"/>.</typeparam>
public class PageableQuerySpecification<T, TResult> : IPageableQuerySpecification<T, TResult> where T : class
{
    /// <summary>
    /// Gets or sets Filter.
    /// </summary>
    public IFilterSpecification<T>? Filter { get; set; }

    /// <summary>
    /// Gets or sets Include.
    /// </summary>
    public IIncludeSpecification<T>? Include { get; set; }

    /// <summary>
    /// Gets or sets Projection.
    /// </summary>
    public IProjectionSpecification<T, TResult>? Projection { get; set; }

    /// <summary>
    /// Gets or sets Order.
    /// </summary>
    public IOrderSpecification<TResult>? Order { get; set; }

    /// <summary>
    /// Gets or sets PagingRequest.
    /// </summary>
    public PagingRequest PagingRequest { get; set; }

    /// <summary>
    /// Constructor takes <see cref="IOrderableQuerySpecification{T}"/> BuildForm and PagingRequest.
    /// </summary>
    /// <param name="buildFrom">Instance of <see cref="IOrderableQuerySpecification{T}"/>.</param>
    /// <param name="sortablePagingRequest">Instance of PagingRequest.</param>
    public PageableQuerySpecification(
        IOrderableQuerySpecification<T, TResult> buildFrom,
        PagingRequest sortablePagingRequest)
    {
        ArgumentNullException.ThrowIfNull(sortablePagingRequest, nameof(sortablePagingRequest));
        ArgumentNullException.ThrowIfNull(buildFrom, nameof(buildFrom));

        Filter = buildFrom.Filter;
        Include = buildFrom.Include;
        Projection = buildFrom.Projection;
        Order = buildFrom.Order;

        PagingRequest = sortablePagingRequest;
    }
}