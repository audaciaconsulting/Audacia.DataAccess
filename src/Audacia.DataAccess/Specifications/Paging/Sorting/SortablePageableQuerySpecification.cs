using Audacia.Core;
using Audacia.DataAccess.Specifications.Filtering;
using Audacia.DataAccess.Specifications.Including;
using Audacia.DataAccess.Specifications.Projection;

namespace Audacia.DataAccess.Specifications.Paging.Sorting;

/// <summary>
/// Default implementation of <see cref="ISortablePageableQuerySpecification{T}"/> that
/// allows a collection of objects of type <see cref="T"/> to be filtered, paged
/// and have navigation properties included in results.
/// </summary>
/// <typeparam name="T"></typeparam>
public class SortablePageableQuerySpecification<T> : ISortablePageableQuerySpecification<T> where T : class
{
    public IFilterSpecification<T> Filter { get; set; }
    public IIncludeSpecification<T> Include { get; set; }
    public SortablePagingRequest SortablePagingRequest { get; set; }

    public SortablePageableQuerySpecification(SortablePagingRequest sortablePagingRequest)
    {
        SortablePagingRequest = sortablePagingRequest;
    }

    public SortablePageableQuerySpecification(IQuerySpecification<T> buildFrom, SortablePagingRequest sortablePagingRequest)
    {
        Filter = buildFrom.Filter;
        Include = buildFrom.Include;
        SortablePagingRequest = sortablePagingRequest;
    }
}

/// <summary>
/// Default implementation of <see cref="ISortablePageableQuerySpecification{T,TResult}"/> that
/// allows a collection of objects of type <see cref="T"/> to be filtered, paged, converted to objects
/// of type <see cref="TResult"/> and have navigation properties included in results .
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TResult"></typeparam>
public class SortablePageableQuerySpecification<T, TResult> : ISortablePageableQuerySpecification<T, TResult> where T : class
{
    public IFilterSpecification<T> Filter { get; set; }
    public IIncludeSpecification<T> Include { get; set; }
    public IProjectionSpecification<T, TResult> Projection { get; set; }
    public SortablePagingRequest SortablePagingRequest { get; set; }

    public SortablePageableQuerySpecification(SortablePagingRequest sortablePagingRequest)
    {
        SortablePagingRequest = sortablePagingRequest;
    }

    public SortablePageableQuerySpecification(IProjectableQuerySpecification<T, TResult> buildFrom, SortablePagingRequest sortablePagingRequest)
    {
        Filter = buildFrom.Filter;
        Include = buildFrom.Include;
        Projection = buildFrom.Projection;
        SortablePagingRequest = sortablePagingRequest;
    }
}