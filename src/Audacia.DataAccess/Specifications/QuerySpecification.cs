using System;
using System.Linq.Expressions;
using Audacia.Core;
using Audacia.DataAccess.Specifications.Filtering;
using Audacia.DataAccess.Specifications.Including;
using Audacia.DataAccess.Specifications.Ordering;
using Audacia.DataAccess.Specifications.Paging.Sorting;
using Audacia.DataAccess.Specifications.Projection;

namespace Audacia.DataAccess.Specifications;

/// <summary>
/// Convenience class to allow <see cref="QuerySpecification{T}"/> instances to be created using type inference
/// so generic parameters don't have to be specified explicitly.
/// </summary>
public static class QuerySpecification
{
    /// <summary>
    /// Creates a <see cref="QuerySpecification{T}"/> for the given type param.
    /// The returned <see cref="QuerySpecification{T}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static QuerySpecification<T> For<T>() where T : class
    {
        return new QuerySpecification<T>();
    }

    /// <summary>
    /// Creates a <see cref="QuerySpecification{T}"/> from the given <paramref name="filterSpecification"/>.
    /// The returned <see cref="QuerySpecification{T}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filterSpecification"></param>
    /// <returns></returns>
    public static QuerySpecification<T> WithFilter<T>(IFilterSpecification<T> filterSpecification) where T : class
    {
        return new QuerySpecification<T>(filterSpecification);
    }

    /// <summary>
    /// Creates a <see cref="QuerySpecification{T}"/> with a <see cref="IFilterSpecification{T}"/> created from the given <paramref name="filterExpression"/>.
    /// The returned <see cref="QuerySpecification{T}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filterExpression"></param>
    /// <returns></returns>
    public static QuerySpecification<T> WithFilter<T>(Expression<Func<T, bool>> filterExpression) where T : class
    {
        return new QuerySpecification<T>(new DynamicFilterSpecification<T>(filterExpression));
    }

    /// <summary>
    /// Creates a <see cref="QuerySpecification{T}"/> from the given <paramref name="includeSpecification"/>.
    /// The returned <see cref="QuerySpecification{T}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="includeSpecification"></param>
    /// <returns></returns>
    public static QuerySpecification<T> WithInclude<T>(IIncludeSpecification<T> includeSpecification) where T : class
    {
        return new QuerySpecification<T>(includeSpecification);
    }

    /// <summary>
    /// Creates a <see cref="QuerySpecification{T}"/> with a <see cref="IIncludeSpecification{T}"/> created from the given <paramref name="includeAction"/>.
    /// The returned <see cref="QuerySpecification{T}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="includeAction"></param>
    /// <returns></returns>
    public static QuerySpecification<T> WithInclude<T>(Action<IBuildableIncludeSpecification<T>> includeAction) where T : class
    {
        return new QuerySpecification<T>(new DynamicIncludeSpecification<T>(includeAction));
    }

    /// <summary>
    /// Creates an <see cref="OrderableQuerySpecification{T}"/> with the given <see cref="IOrderSpecification{T}"/>.
    /// The returned <see cref="OrderableQuerySpecification{T}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="orderBySpecification"></param>
    /// <returns></returns>
    public static OrderableQuerySpecification<T> WithOrder<T>(IOrderSpecification<T> orderBySpecification) where T : class
    {
        return new OrderableQuerySpecification<T>(orderBySpecification);
    }

    /// <summary>
    /// Creates a <see cref="QuerySpecification{T}"/> with a <see cref="IOrderSpecification{T}"/> created from the given <paramref name="orderAction"/>.
    /// The returned <see cref="QuerySpecification{T}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="orderAction"></param>
    /// <returns></returns>
    public static QuerySpecification<T> WithOrder<T>(Action<IBuildableOrderSpecification<T>> orderAction) where T : class
    {
        return new QuerySpecification<T>(new DynamicOrderSpecification<T>(orderAction));
    }

    /// <summary>
    /// Creates a <see cref="ProjectableQuerySpecification{T,TResult}"/> with the given <see cref="IProjectionSpecification{T,TResult}"/>.
    /// The returned <see cref="ProjectableQuerySpecification{T,TResult}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projectionSpecification"></param>
    /// <returns></returns>
    public static ProjectableQuerySpecification<T, TResult> WithProjection<T, TResult>(IProjectionSpecification<T, TResult> projectionSpecification) where T : class where TResult : class
    {
        return new ProjectableQuerySpecification<T, TResult>(projectionSpecification);
    }
    
    public static ProjectableQuerySpecification<T, TResult> WithProjection<T, TResult>(Expression<Func<T, TResult>> expression) where T : class where TResult : class
    {
        return new ProjectableQuerySpecification<T, TResult>(new DynamicProjectionSpecification<T, TResult>(expression));
    }

    /// <summary>
    /// Creates a <see cref="SortablePageableQuerySpecification{T}"/> with the given <see cref="SortablePagingRequest"/>.
    /// The returned <see cref="SortablePageableQuerySpecification{T}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sortablePagingRequest"></param>
    /// <returns></returns>
    public static SortablePageableQuerySpecification<T> WithPaging<T>(SortablePagingRequest sortablePagingRequest) where T : class
    {
        return new SortablePageableQuerySpecification<T>(sortablePagingRequest);
    }

    /// <summary>
    /// Creates a <see cref="SortablePageableQuerySpecification{T,TResult}"/> with the given <see cref="SortablePagingRequest"/>.
    /// The returned <see cref="SortablePageableQuerySpecification{T,TResult}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="sortablePagingRequest"></param>
    /// <returns></returns>
    public static SortablePageableQuerySpecification<T, TResult> WithPaging<T, TResult>(SortablePagingRequest sortablePagingRequest) where T : class where TResult : class
    {
        return new SortablePageableQuerySpecification<T, TResult>(sortablePagingRequest);
    }
}

/// <summary>
/// Default implementation of <see cref="IQuerySpecification{T}"/> via <see cref="IOrderableQuerySpecification{T}"/> that can be used to
/// build specifications containing filtering, ordering and the including of navigation properties.
/// </summary>
/// <typeparam name="T"></typeparam>
public class QuerySpecification<T> : IOrderableQuerySpecification<T>
    where T : class
{
    public IFilterSpecification<T> Filter { get; set; }

    public IIncludeSpecification<T> Include { get; set; }

    public IOrderSpecification<T> Order { get; set; }

    public QuerySpecification()
    {
    }

    public QuerySpecification(IQuerySpecification<T> querySpecification)
    {
        Filter = querySpecification.Filter;
        Include = querySpecification.Include;
    }

    public QuerySpecification(IFilterSpecification<T> filterSpecification)
    {
        Filter = filterSpecification;
    }

    public QuerySpecification(IIncludeSpecification<T> includeSpecification)
    {
        Include = includeSpecification;
    }

    public QuerySpecification(IOrderSpecification<T> orderSpecification)
    {
        Order = orderSpecification;
    }
}