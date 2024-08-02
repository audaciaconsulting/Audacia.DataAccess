using System;
using System.Collections.Generic;

namespace Audacia.DataAccess.Specifications.Ordering;

/// <summary>
/// Implements <see cref="IOrderSpecification{T}"/>.
/// </summary>
/// <typeparam name="T">Type of <see cref="IOrderSpecification{T}"/>.</typeparam>
public class DynamicOrderSpecification<T> : IOrderSpecification<T>
{
    private readonly IBuildableOrderSpecification<T> _wrappedSpecification;

    /// <summary>
    /// Constructor which adds orderAction to existing <see cref="IBuildableOrderSpecification{T}"/> instance.
    /// </summary>
    /// <param name="orderAction">Instance of <see cref="IBuildableOrderSpecification{T}"/>.</param>
    public DynamicOrderSpecification(Action<IBuildableOrderSpecification<T>> orderAction)
    {
        if (orderAction is null)
        {
            throw new ArgumentNullException(nameof(orderAction));
        }

        _wrappedSpecification = OrderSpecification<T>.CreateInternal();
    }

    /// <summary>
    /// Gets OrderSteps.
    /// </summary>
    public IEnumerable<OrderStep> OrderSteps => _wrappedSpecification.OrderSteps;
}