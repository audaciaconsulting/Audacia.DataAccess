using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Ordering;

/// <summary>
/// Class that allows <see cref="IOrderSpecification{T}"/> objects to be built and then executed using the <see cref="OrderSteps"/> property.
/// </summary>
/// <typeparam name="T">Type of <see cref="IBuildableOrderSpecification{T}"/>.</typeparam>
public class OrderSpecification<T> : IBuildableOrderSpecification<T>, IBuildableOrderedOrderSpecification<T>
{
    private List<OrderStep> _orderSteps = new List<OrderStep>();

    /// <summary>
    /// Gets OrderSteps.
    /// </summary>
    public IEnumerable<OrderStep> OrderSteps => _orderSteps;

    /// <summary>
    /// Empty constructor.
    /// </summary>
    protected OrderSpecification()
    {
    }

    /// <summary>
    /// Creates a new <see cref="OrderSpecification{T}"/>.
    /// </summary>
    /// <returns><see cref="OrderSpecification{T}"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000: Do not declare static members on generic types", Justification = "Code is already in use and will introduce breaking changes.")]
    public static OrderSpecification<T> CreateInternal()
    {
        return new OrderSpecification<T>();
    }

    /// <summary>
    /// Factory method to create an <see cref="OrderSpecification{T}"/> from existing <see cref="IOrderSpecification{T}"/>s.
    /// Provides safety by returning the created specification as an <see cref="IBuildableOrderedOrderSpecification{T}"/> thus ensuring
    /// that only the appropriate methods can be called on the returned object.
    /// </summary>
    /// <param name="existingSpecifications">Instance of <see cref="IOrderSpecification{T}"/>.</param>
    /// <returns><see cref="IBuildableOrderedOrderSpecification{T}"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000: Do not declare static members on generic types", Justification = "Code is already in use and will introduce breaking changes.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Spacing Rules", "SA1010:Opening Square Brackets Must Be Spaced Correctly", Justification = "This is the only way to create an empty array.")]
    public static IBuildableOrderedOrderSpecification<T> From(params IOrderSpecification<T>[] existingSpecifications)
    {
        var newSpec = new OrderSpecification<T>();
        foreach (var orderSpecification in existingSpecifications ?? [])
        {
            newSpec._orderSteps.AddRange(orderSpecification.OrderSteps);
        }

        return newSpec;
    }

    /// <summary>
    /// Adds the Ascending order specification.
    /// </summary>
    /// <typeparam name="TKey">Return type of <see cref="Expression{TKey}"/>.</typeparam>
    /// <param name="keySelector">Instance of <see cref="Expression{T}"/>.</param>
    /// <returns><see cref="IBuildableOrderedOrderSpecification{T}"/>.</returns>
    public IBuildableOrderedOrderSpecification<T> Asc<TKey>(Expression<Func<T, TKey>> keySelector)
    {
        _orderSteps = new List<OrderStep> { new OrderStep(true, typeof(TKey), keySelector) };

        return this;
    }

    /// <summary>
    /// Adds descending order specification.
    /// </summary>
    /// <typeparam name="TKey">Return type of <see cref="Expression{TKey}"/>.</typeparam>
    /// <param name="keySelector">Instance of <see cref="Expression{T}"/>.</param>
    /// <returns><see cref="IBuildableOrderedOrderSpecification{T}"/>.</returns>
    public IBuildableOrderedOrderSpecification<T> Desc<TKey>(Expression<Func<T, TKey>> keySelector)
    {
        _orderSteps = new List<OrderStep> { new OrderStep(false, typeof(TKey), keySelector) };

        return this;
    }

    /// <summary>
    /// Add further ascending order speacifications.
    /// </summary>
    /// <typeparam name="TKey">Return type of <see cref="Expression{TKey}"/>.</typeparam>
    /// <param name="keySelector">Instance of <see cref="Expression{T}"/>.</param>
    /// <returns><see cref="IBuildableOrderedOrderSpecification{T}"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "Breaking change.")]
    IBuildableOrderedOrderSpecification<T> IBuildableOrderedOrderSpecification<T>.ThenAsc<TKey>(Expression<Func<T, TKey>> keySelector)
    {
        _orderSteps.Add(new OrderStep(true, typeof(TKey), keySelector));

        return this;
    }

    /// <summary>
    /// Add further descending order specifications.
    /// </summary>
    /// <typeparam name="TKey">Return type of <see cref="Expression{TKey}"/>.</typeparam>
    /// <param name="keySelector">Instance of <see cref="Expression{T}"/>.</param>
    /// <returns><see cref="IBuildableOrderedOrderSpecification{T}"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "Breaking change.")]
    IBuildableOrderedOrderSpecification<T> IBuildableOrderedOrderSpecification<T>.ThenDesc<TKey>(Expression<Func<T, TKey>> keySelector)
    {
        _orderSteps.Add(new OrderStep(false, typeof(TKey), keySelector));

        return this;
    }
}