using Audacia.Core;
using Audacia.DataAccess.Specifications.Filtering;
using Audacia.DataAccess.Specifications.Including;
using Audacia.DataAccess.Specifications.Projection;

namespace Audacia.DataAccess.Specifications.Paging.Sorting;

/// <summary>
/// Default implementation of <see cref="ISortablePageableQuerySpecification{T}"/> that
/// allows a collection of objects of type <see cref="ISortablePageableQuerySpecification{T}"/> to be filtered, paged
/// and have navigation properties included in results.
/// </summary>
/// <typeparam name="T">Type of <see cref="ISortablePageableQuerySpecification{T}"/>.</typeparam>
public class SortablePageableQuerySpecification<T> : ISortablePageableQuerySpecification<T> where T : class
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
    /// Gets or sets SortablePagingRequest.
    /// </summary>
    public SortablePagingRequest SortablePagingRequest { get; set; }

    /// <summary>
    /// Constructor which takes in an instance of <see cref="SortablePagingRequest"/>.
    /// </summary>
    /// <param name="sortablePagingRequest">Instance of <see cref="SortablePagingRequest"/>.</param>
    public SortablePageableQuerySpecification(SortablePagingRequest sortablePagingRequest)
    {
        SortablePagingRequest = sortablePagingRequest;
    }

    /// <summary>
    /// Constructor takes in <see cref="IQuerySpecification{T}"/> and <see cref="SortablePagingRequest"/>.
    /// </summary>
    /// <param name="buildFrom">Instance of  <see cref="IQuerySpecification{T}"/>.</param>
    /// <param name="sortablePagingRequest">Instance of <see cref="SortablePagingRequest"/>.</param>
    public SortablePageableQuerySpecification(IQuerySpecification<T> buildFrom, SortablePagingRequest sortablePagingRequest)
    {
        if (buildFrom != null)
        {
            Filter = buildFrom.Filter;
            Include = buildFrom.Include;
        }

        SortablePagingRequest = sortablePagingRequest;
    }
}

/// <summary>
/// Default implementation of <see cref="ISortablePageableQuerySpecification{T,TResult}"/> that
/// allows a collection of objects of type <see cref="ISortablePageableQuerySpecification{T}"/> to be filtered, paged, converted to objects
/// of type <see cref="ISortablePageableQuerySpecification{TResult}"/> and have navigation properties included in results .
/// </summary>
/// <typeparam name="T">Type of <see cref="ISortablePageableQuerySpecification{T}"/>.</typeparam>
/// <typeparam name="TResult">Return type of <see cref="ISortablePageableQuerySpecification{TResult}"/>.</typeparam>
public class SortablePageableQuerySpecification<T, TResult> : ISortablePageableQuerySpecification<T, TResult> where T : class
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
    /// Gets or sets SortablePagingRequest.
    /// </summary>
    public SortablePagingRequest SortablePagingRequest { get; set; }

    /// <summary>
    /// Constructor takes in <see cref="SortablePagingRequest"/>.
    /// </summary>
    /// <param name="sortablePagingRequest">Instance of <see cref="SortablePagingRequest"/>.</param>
    public SortablePageableQuerySpecification(SortablePagingRequest sortablePagingRequest)
    {
        SortablePagingRequest = sortablePagingRequest;
    }

    /// <summary>
    /// Constructor takes in <see cref="IProjectableQuerySpecification{T,TResult}"/> and <see cref="SortablePagingRequest"/>.
    /// </summary>
    /// <param name="buildFrom">Instance of <see cref="IProjectableQuerySpecification{T,TResult}"/>.</param>
    /// <param name="sortablePagingRequest">Instance of <see cref="SortablePagingRequest"/>.</param>
    public SortablePageableQuerySpecification(IProjectableQuerySpecification<T, TResult> buildFrom, SortablePagingRequest sortablePagingRequest)
    {
        if (buildFrom != null)
        {
            Filter = buildFrom.Filter;
            Include = buildFrom.Include;
            Projection = buildFrom.Projection;
        }

        SortablePagingRequest = sortablePagingRequest;
    }
}