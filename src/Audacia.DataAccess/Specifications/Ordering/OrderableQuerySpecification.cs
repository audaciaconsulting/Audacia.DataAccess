using System;
using Audacia.DataAccess.Specifications.Filtering;
using Audacia.DataAccess.Specifications.Including;
using Audacia.DataAccess.Specifications.Projection;

namespace Audacia.DataAccess.Specifications.Ordering;

/// <summary>
/// The default implementation of <see cref="IOrderableQuerySpecification{T}"/> that contains the base <see cref="IQuerySpecification{T}"/> properties
/// together with an <see cref="IOrderSpecification{T}"/>.
/// </summary>
/// <typeparam name="T">Type of <see cref="IOrderableQuerySpecification{T}"/>.</typeparam>
public class OrderableQuerySpecification<T> : IOrderableQuerySpecification<T>
{
    /// <summary>
    /// Gets or sets <see cref="IFilterSpecification{T}"/> Filter.
    /// </summary>
    public IFilterSpecification<T>? Filter { get; set; }

    /// <summary>
    /// Gets or sets <see cref="IIncludeSpecification{T}"/> Include.
    /// </summary>
    public IIncludeSpecification<T>? Include { get; set; }

    /// <summary>
    /// Gets or sets <see cref="IOrderSpecification{T}"/> Order.
    /// </summary>
    public IOrderSpecification<T>? Order { get; set; }

    /// <summary>
    /// Constructor which takes an instance of <see cref="IOrderSpecification{T}"/>.
    /// </summary>
    /// <param name="orderSpecification"><see cref="IOrderSpecification{T}"/>.</param>
    public OrderableQuerySpecification(IOrderSpecification<T> orderSpecification)
    {
        if (orderSpecification is null)
        {
            throw new ArgumentNullException(nameof(orderSpecification));
        }

        Order = orderSpecification;
    }

    /// <summary>
    /// Constructor which takes an instance of <see cref="IQuerySpecification{T}"/> and <see cref="IOrderSpecification{T}"/>.
    /// </summary>
    /// <param name="buildFrom">Instance of <see cref="IQuerySpecification{T}"/>.</param>
    /// <param name="orderSpecification">Instance of <see cref="IOrderSpecification{T}"/>.</param>
    public OrderableQuerySpecification(IQuerySpecification<T> buildFrom, IOrderSpecification<T> orderSpecification)
    {
        if (orderSpecification is null)
        {
            throw new ArgumentNullException(nameof(orderSpecification));
        }

        if (buildFrom is null)
        {
            throw new ArgumentNullException(nameof(buildFrom));
        }

        Filter = buildFrom.Filter;
        Include = buildFrom.Include;

        Order = orderSpecification;
    }
}

/// <summary>
/// The default implementation of <see cref="IOrderableQuerySpecification{T,TResult}"/> that contains 
/// the base <see cref="IProjectableQuerySpecification{T,TResult}"/> properties together with an <see cref="IOrderSpecification{T}"/>.
/// </summary>
/// <typeparam name="T">Type of <see cref="IOrderableQuerySpecification{T}"/>.</typeparam>
/// <typeparam name="TResult">Return type of <see cref="IOrderableQuerySpecification{TResult}"/>.</typeparam>
public class OrderableQuerySpecification<T, TResult> : IOrderableQuerySpecification<T, TResult> where T : class
{
    /// <summary>
    /// Gets or sets <see cref="IFilterSpecification{T}"/> Filter.
    /// </summary>
    public IFilterSpecification<T>? Filter { get; set; }

    /// <summary>
    /// Gets or sets <see cref="IIncludeSpecification{T}"/> Include.
    /// </summary>
    public IIncludeSpecification<T>? Include { get; set; }

    /// <summary>
    /// Gets or sets <see cref="IProjectionSpecification{T,TResult}"/> Projection.
    /// </summary>
    public IProjectionSpecification<T, TResult>? Projection { get; set; }

    /// <summary>
    /// Gets or sets <see cref="IOrderSpecification{TResult}"/> Order.
    /// </summary>
    public IOrderSpecification<TResult>? Order { get; set; }

    /// <summary>
    /// Constructor which takes  <see cref="IOrderSpecification{TResult}"/>  <see cref="IOrderSpecification{TResult}"/>.
    /// </summary>
    /// <param name="buildFrom"> <see cref="IProjectableQuerySpecification{T, TResult}"/>.</param>
    /// <param name="orderSpecification"> <see cref="IOrderSpecification{TResult}"/>.</param>
    public OrderableQuerySpecification(
        IProjectableQuerySpecification<T, TResult> buildFrom,
        IOrderSpecification<TResult> orderSpecification)
    {
        if (buildFrom != null)
        {
            Filter = buildFrom.Filter;
            Include = buildFrom.Include;
            Projection = buildFrom.Projection;
        }

        Order = orderSpecification;
    }
}