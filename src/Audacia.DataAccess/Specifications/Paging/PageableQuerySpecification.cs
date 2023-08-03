using Audacia.Core;
using Audacia.DataAccess.Specifications.Filtering;
using Audacia.DataAccess.Specifications.Including;
using Audacia.DataAccess.Specifications.Ordering;
using Audacia.DataAccess.Specifications.Projection;

namespace Audacia.DataAccess.Specifications.Paging;

/// <summary>
/// Default implementation of <see cref="IPageableQuerySpecification{T}"/> that
/// allows a collection of objects of type <see cref="T"/> to be filtered, paged
/// and have navigation properties included in results.
/// </summary>
/// <typeparam name="T"></typeparam>
public class PageableQuerySpecification<T> : IPageableQuerySpecification<T> where T : class
{
    public IFilterSpecification<T> Filter { get; set; }

    public IIncludeSpecification<T> Include { get; set; }

    public IOrderSpecification<T> Order { get; set; }

    public PagingRequest PagingRequest { get; set; }

    public PageableQuerySpecification(IOrderableQuerySpecification<T> buildFrom,
        PagingRequest sortablePagingRequest)
    {
        Filter = buildFrom.Filter;
        Include = buildFrom.Include;
        Order = buildFrom.Order;
        PagingRequest = sortablePagingRequest;
    }
}

/// <summary>
/// Default implementation of <see cref="IPageableQuerySpecification{T,TResult}"/> that
/// allows a collection of objects of type <see cref="T"/> to be filtered, paged, converted to objects
/// of type <see cref="TResult"/> and have navigation properties included in results .
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TResult"></typeparam>
public class PageableQuerySpecification<T, TResult> : IPageableQuerySpecification<T, TResult> where T : class
{
    public IFilterSpecification<T> Filter { get; set; }

    public IIncludeSpecification<T> Include { get; set; }

    public IProjectionSpecification<T, TResult> Projection { get; set; }

    public IOrderSpecification<TResult> Order { get; set; }

    public PagingRequest PagingRequest { get; set; }

    public PageableQuerySpecification(IOrderableQuerySpecification<T, TResult> buildFrom,
        PagingRequest sortablePagingRequest)
    {
        Filter = buildFrom.Filter;
        Include = buildFrom.Include;
        Projection = buildFrom.Projection;
        Order = buildFrom.Order;
        PagingRequest = sortablePagingRequest;
    }
}