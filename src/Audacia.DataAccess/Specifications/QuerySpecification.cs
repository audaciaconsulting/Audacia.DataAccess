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
    /// <typeparam name="T">Type of <see cref="QuerySpecification{T}"/>.</typeparam>
    /// <returns>Instance of <see cref="QuerySpecification{T}" />.</returns>
    public static QuerySpecification<T> For<T>() where T : class
    {
        return new QuerySpecification<T>();
    }

    /// <summary>
    /// Creates a <see cref="QuerySpecification{T}"/> from the given <paramref name="filterSpecification"/>.
    /// The returned <see cref="QuerySpecification{T}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IFilterSpecification{T}"/>.</typeparam>
    /// <param name="filterSpecification"><see cref="IFilterSpecification{T}"/>.</param>
    /// <returns>Instance of <see cref="QuerySpecification{T}" />.</returns>
    public static QuerySpecification<T> WithFilter<T>(IFilterSpecification<T> filterSpecification) where T : class
    {
        return new QuerySpecification<T>(filterSpecification);
    }

    /// <summary>
    /// Creates a <see cref="QuerySpecification{T}"/> with a <see cref="IFilterSpecification{T}"/> created from the given <paramref name="filterExpression"/>.
    /// The returned <see cref="QuerySpecification{T}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="Expression{T}"/>.</typeparam>
    /// <param name="filterExpression"><see cref="Expression{T}"/>.</param>
    /// <returns>Instance of <see cref="QuerySpecification{T}" />.</returns>
    public static QuerySpecification<T> WithFilter<T>(Expression<Func<T, bool>> filterExpression) where T : class
    {
        return new QuerySpecification<T>(new DynamicFilterSpecification<T>(filterExpression));
    }

    /// <summary>
    /// Creates a <see cref="QuerySpecification{T}"/> from the given <paramref name="includeSpecification"/>.
    /// The returned <see cref="QuerySpecification{T}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IIncludeSpecification{T}"/>.</typeparam>
    /// <param name="includeSpecification"><see cref="IIncludeSpecification{T}"/>.</param>
    /// <returns>Instance of <see cref="QuerySpecification{T}" />.</returns>
    public static QuerySpecification<T> WithInclude<T>(IIncludeSpecification<T> includeSpecification) where T : class
    {
        return new QuerySpecification<T>(includeSpecification);
    }

    /// <summary>
    /// Creates a <see cref="QuerySpecification{T}"/> with a <see cref="IIncludeSpecification{T}"/> created from the given <paramref name="includeAction"/>.
    /// The returned <see cref="QuerySpecification{T}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IBuildableIncludeSpecification{T}"/>.</typeparam>
    /// <param name="includeAction"><see cref="IBuildableIncludeSpecification{T}"/>.</param>
    /// <returns>Instance of <see cref="QuerySpecification{T}" />.</returns>
    public static QuerySpecification<T> WithInclude<T>(Action<IBuildableIncludeSpecification<T>> includeAction) where T : class
    {
        return new QuerySpecification<T>(new DynamicIncludeSpecification<T>(includeAction));
    }

    /// <summary>
    /// Creates an <see cref="OrderableQuerySpecification{T}"/> with the given <see cref="IOrderSpecification{T}"/>.
    /// The returned <see cref="OrderableQuerySpecification{T}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IOrderSpecification{T}"/>.</typeparam>
    /// <param name="orderBySpecification"><see cref="IOrderSpecification{T}"/>.</param>
    /// <returns>Instance of <see cref="OrderableQuerySpecification{T}" />.</returns>
    public static OrderableQuerySpecification<T> WithOrder<T>(IOrderSpecification<T> orderBySpecification) where T : class
    {
        return new OrderableQuerySpecification<T>(orderBySpecification);
    }

    /// <summary>
    /// Creates a <see cref="QuerySpecification{T}"/> with a <see cref="IOrderSpecification{T}"/> created from the given <paramref name="orderAction"/>.
    /// The returned <see cref="QuerySpecification{T}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IBuildableOrderSpecification{T}"/>.</typeparam>
    /// <param name="orderAction"><see cref="IBuildableOrderSpecification{T}"/>.</param>
    /// <returns>Instance of <see cref="QuerySpecification{T}" />.</returns>
    public static QuerySpecification<T> WithOrder<T>(Action<IBuildableOrderSpecification<T>> orderAction) where T : class
    {
        return new QuerySpecification<T>(new DynamicOrderSpecification<T>(orderAction));
    }

    /// <summary>
    /// Creates a <see cref="ProjectableQuerySpecification{T,TResult}"/> with the given <see cref="IProjectionSpecification{T,TResult}"/>.
    /// The returned <see cref="ProjectableQuerySpecification{T,TResult}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IProjectionSpecification{T, TResult}"/>.</typeparam>
    /// <typeparam name="TResult">Return type of <see cref="IProjectionSpecification{T, TResult}"/>.</typeparam>
    /// <param name="projectionSpecification"><see cref="IProjectionSpecification{T, TResult}"/>.</param>    
    /// <returns>Instance of <see cref="ProjectableQuerySpecification{T, TResult}" />.</returns>
    public static ProjectableQuerySpecification<T, TResult> WithProjection<T, TResult>(IProjectionSpecification<T, TResult> projectionSpecification) where T : class where TResult : class
    {
        return new ProjectableQuerySpecification<T, TResult>(projectionSpecification);
    }

    /// <summary>
    /// Creates a <see cref="ProjectableQuerySpecification{T,TResult}"/> with the given <see cref="Expression{Func}"/>.
    /// The returned <see cref="ProjectableQuerySpecification{T,TResult}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="Expression{T}"/>.</typeparam>
    /// <typeparam name="TResult">Return type of <see cref="Expression{TResult}"/>.</typeparam>
    /// <param name="expression">Expression value.</param>
    /// <returns>Instance of <see cref="ProjectableQuerySpecification{T, TResult}" />.</returns>
    public static ProjectableQuerySpecification<T, TResult> WithProjection<T, TResult>(Expression<Func<T, TResult>> expression) where T : class where TResult : class
    {
        return new ProjectableQuerySpecification<T, TResult>(new DynamicProjectionSpecification<T, TResult>(expression));
    }

    /// <summary>
    /// Creates a <see cref="SortablePageableQuerySpecification{T}"/> with the given <see cref="SortablePagingRequest"/>.
    /// The returned <see cref="SortablePageableQuerySpecification{T}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="SortablePageableQuerySpecification{T}"/>.</typeparam>
    /// <param name="sortablePagingRequest"><see cref="SortablePagingRequest"/>.</param>
    /// <returns>Instance of <see cref="SortablePageableQuerySpecification{T}" />.</returns>
    public static SortablePageableQuerySpecification<T> WithPaging<T>(SortablePagingRequest sortablePagingRequest) where T : class
    {
        return new SortablePageableQuerySpecification<T>(sortablePagingRequest);
    }

    /// <summary>
    /// Creates a <see cref="SortablePageableQuerySpecification{T,TResult}"/> with the given <see cref="SortablePagingRequest"/>.
    /// The returned <see cref="SortablePageableQuerySpecification{T,TResult}"/> can be used to add further specifications.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="SortablePageableQuerySpecification{T}"/>.</typeparam>
    /// <typeparam name="TResult">Return type of <see cref="SortablePageableQuerySpecification{TResult}"/>.</typeparam>
    /// <param name="sortablePagingRequest"><see cref="SortablePagingRequest"/>.</param>
    /// <returns>Instance of <see cref="SortablePageableQuerySpecification{T, TResult}" />.</returns>
    public static SortablePageableQuerySpecification<T, TResult> WithPaging<T, TResult>(SortablePagingRequest sortablePagingRequest) where T : class where TResult : class
    {
        return new SortablePageableQuerySpecification<T, TResult>(sortablePagingRequest);
    }
}

/// <summary>
/// Default implementation of <see cref="IQuerySpecification{T}"/> via <see cref="IOrderableQuerySpecification{T}"/> that can be used to
/// build specifications containing filtering, ordering and the including of navigation properties.
/// </summary>
/// <typeparam name="T">Type of QuerySpecification.</typeparam>
public class QuerySpecification<T> : IOrderableQuerySpecification<T>
    where T : class
{
    /// <summary>
    /// Gets or sets Filter value.
    /// </summary>
    public IFilterSpecification<T>? Filter { get; set; }

    /// <summary>
    /// Gets or sets Include value.
    /// </summary>
    public IIncludeSpecification<T>? Include { get; set; }

    /// <summary>
    /// Gets or sets Order value.
    /// </summary>
    public IOrderSpecification<T>? Order { get; set; }

    /// <summary>
    /// Empty constructor.
    /// </summary>
    public QuerySpecification()
    {
    }

    /// <summary> Constructor for setting up QuerySpecification values. </summary>
    /// <param name="querySpecification">QuerySpecification values.</param>
    public QuerySpecification(IQuerySpecification<T> querySpecification)
    {
        Filter = querySpecification?.Filter;
        Include = querySpecification?.Include;
    }

    /// <summary>Constructor for setting up FilterSpecification values.</summary>
    /// <param name="filterSpecification">FilterSpecification value.</param>
    public QuerySpecification(IFilterSpecification<T> filterSpecification)
    {
        Filter = filterSpecification;
    }

    /// <summary> Constructor for setting up IncludeSpecification values.</summary>
    /// <param name="includeSpecification">IncludeSpecification value.</param>
    public QuerySpecification(IIncludeSpecification<T> includeSpecification)
    {
        Include = includeSpecification;
    }

    /// <summary> Constructor for setting up OrderSpecification values.</summary>
    /// <param name="orderSpecification">OrderSpecification value.</param>
    public QuerySpecification(IOrderSpecification<T> orderSpecification)
    {
        Order = orderSpecification;
    }
}