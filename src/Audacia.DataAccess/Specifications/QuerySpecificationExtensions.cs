using System;
using System.Linq.Expressions;
using Audacia.Core;
using Audacia.DataAccess.Specifications.Filtering;
using Audacia.DataAccess.Specifications.Including;
using Audacia.DataAccess.Specifications.Ordering;
using Audacia.DataAccess.Specifications.Paging;
using Audacia.DataAccess.Specifications.Paging.Sorting;
using Audacia.DataAccess.Specifications.Projection;

namespace Audacia.DataAccess.Specifications;

public static class QuerySpecificationExtensions
{
    public static IQuerySpecification<T> AddFilter<T>(this IQuerySpecification<T> querySpecification,
        IFilterSpecification<T> filterSpecification)
        where T : class
    {
        querySpecification.Filter = querySpecification.Filter == null
            ? filterSpecification
            : querySpecification.Filter.And(filterSpecification);

        return querySpecification;
    }

    public static IQuerySpecification<T> AddFilter<T>(this IQuerySpecification<T> querySpecification,
        Expression<Func<T, bool>> filterExpression)
        where T : class
    {
        var filterSpecification = new DynamicFilterSpecification<T>(filterExpression);

        return querySpecification.AddFilter(filterSpecification);
    }

    /// <summary>
    /// <para>
    /// Adds the given <paramref name="includeSpecification"/> to the given <paramref name="querySpecification"/>.
    /// </para>
    /// <para>
    /// If the <paramref name="querySpecification"/> already has an <see cref="IIncludeSpecification{T}"/> then the given
    /// <paramref name="includeSpecification"/> will be added in addition to the existing specification(s).
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> instance to which to add the specification.</param>
    /// <param name="includeSpecification">The <see cref="IIncludeSpecification{T}"/> instance to add to the query.</param>
    /// <returns></returns>
    public static IOrderableQuerySpecification<T> WithInclude<T>(this IQuerySpecification<T> querySpecification,
        IIncludeSpecification<T> includeSpecification)
        where T : class
    {
        if (querySpecification.Include == null)
        {
            querySpecification.Include = includeSpecification;
        }
        else
        {
            querySpecification.Include = IncludeSpecification<T>.From(
                querySpecification.Include, includeSpecification);
        }

        if (querySpecification is IOrderableQuerySpecification<T> orderableQuerySpecification)
        {
            return orderableQuerySpecification;
        }

        return new QuerySpecification<T>(querySpecification);
    }

    /// <summary>
    /// <para>
    /// Adds an <see cref="IIncludeSpecification{T}"/> built from the given <paramref name="includeAction"/> to the given <paramref name="querySpecification"/>.
    /// </para>
    /// <para>
    /// If the <paramref name="querySpecification"/> already has an <see cref="IIncludeSpecification{T}"/> then the result
    /// of the given <paramref name="includeAction"/> will be added in addition to the existing specification(s).
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> instance to which to add the specification.</param>
    /// <param name="includeAction">The action to be used to build an <see cref="IIncludeSpecification{T}"/> to add to the query.</param>
    /// <returns></returns>
    public static IOrderableQuerySpecification<T> WithInclude<T>(this IQuerySpecification<T> querySpecification,
        Action<IBuildableIncludeSpecification<T>> includeAction)
        where T : class
    {
        var includeSpecification = new DynamicIncludeSpecification<T>(includeAction);

        return querySpecification.WithInclude(includeSpecification);
    }
    
    /// <summary>
    /// Returns the given <paramref name="querySpecification"/> as an instance of <see cref="IOrderableQuerySpecification{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="querySpecification">The specification to convert to an <see cref="IOrderableQuerySpecification{T}"/>.</param>
    /// <returns></returns>
    public static IOrderableQuerySpecification<T> AsOrderable<T>(this IQuerySpecification<T> querySpecification)
        where T : class
    {
        if (querySpecification is IOrderableQuerySpecification<T> orderableQuerySpecification)
        {
            return orderableQuerySpecification;
        }

        return new QuerySpecification<T>(querySpecification);
    }

    /// <summary>
    /// <para>
    /// Adds the given <paramref name="orderSpecification"/> to the given <paramref name="querySpecification"/>.
    /// </para>
    /// <para>
    /// If the <paramref name="querySpecification"/> already has an <see cref="IOrderSpecification{T}"/>
    /// then the given <paramref name="orderSpecification"/> will be added in addition to the existing specification(s).
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> instance to which to add the specification.</param>
    /// <param name="orderSpecification">The <see cref="IOrderSpecification{T}"/> instance to add to the query.</param>
    /// <returns></returns>
    public static IOrderableQuerySpecification<T> WithOrder<T>(
        this IOrderableQuerySpecification<T> querySpecification, IOrderSpecification<T> orderSpecification)
    {
        if (querySpecification.Order == null)
        {
            querySpecification.Order = orderSpecification;
        }
        else
        {
            querySpecification.Order = OrderSpecification<T>.From(querySpecification.Order, orderSpecification);
        }

        return querySpecification;
    }

    /// <summary>
    /// <para>
    /// Adds an <see cref="IOrderSpecification{T}"/> built from the given <paramref name="orderAction"/> to the given <paramref name="querySpecification"/>.
    /// </para>
    /// <para>
    /// If the <paramref name="querySpecification"/> already has an <see cref="IOrderSpecification{T}"/> then the result
    /// of the given <paramref name="orderAction"/> will be added in addition to the existing specification(s).
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> instance to which to add the specification.</param>
    /// <param name="orderAction">The action to be used to build an <see cref="IOrderSpecification{T}"/> to add to the query.</param>
    /// <returns></returns>
    public static IOrderableQuerySpecification<T> WithOrder<T>(
        this IOrderableQuerySpecification<T> querySpecification,
        Action<IBuildableOrderSpecification<T>> orderAction)
    {
        var orderSpecification = new DynamicOrderSpecification<T>(orderAction);

        return querySpecification.WithOrder(orderSpecification);
    }

    public static IProjectableQuerySpecification<T, TResult> WithProjection<T, TResult>(
        this IQuerySpecification<T> querySpecification,
        IProjectionSpecification<T, TResult> projectionSpecification) where T : class
    {
        return new ProjectableQuerySpecification<T, TResult>(querySpecification, projectionSpecification);
    }

    public static IProjectableQuerySpecification<T, TResult> WithProjection<T, TResult>(
        this IQuerySpecification<T> querySpecification, Expression<Func<T, TResult>> projectionExpression)
        where T : class
    {
        return new ProjectableQuerySpecification<T, TResult>(querySpecification,
            new DynamicProjectionSpecification<T, TResult>(projectionExpression));
    }

    public static ISortablePageableQuerySpecification<T> WithPaging<T>(
        this IQuerySpecification<T> querySpecification,
        SortablePagingRequest sortablePagingRequest) where T : class
    {
        return new SortablePageableQuerySpecification<T>(querySpecification, sortablePagingRequest);
    }

    public static IPageableQuerySpecification<T> WithPaging<T>(
        this IOrderableQuerySpecification<T> querySpecification,
        PagingRequest pagingRequest) where T : class
    {
        return new PageableQuerySpecification<T>(querySpecification, pagingRequest);
    }

    public static IPageableQuerySpecification<T, TResult> WithPaging<T, TResult>(
        this IOrderableQuerySpecification<T, TResult> querySpecification,
        PagingRequest pagingRequest) where T : class
    {
        return new PageableQuerySpecification<T, TResult>(querySpecification, pagingRequest);
    }

    public static IOrderableQuerySpecification<T, TResult> WithOrder<T, TResult>(
        this IProjectableQuerySpecification<T, TResult> querySpecification,
        IOrderSpecification<TResult> orderSpecification) where T : class
    {
        return new OrderableQuerySpecification<T, TResult>(querySpecification, orderSpecification);
    }

    public static SortablePageableQuerySpecification<T, TResult> WithPaging<T, TResult>(
        this IProjectableQuerySpecification<T, TResult> querySpecification,
        SortablePagingRequest sortablePagingRequest) where T : class
    {
        return new SortablePageableQuerySpecification<T, TResult>(querySpecification, sortablePagingRequest);
    }
}