using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Ordering;

/// <summary>
/// Encapsulates the data needed to perform an order by clause.
/// </summary>
public class OrderStep
{
    /// <summary>
    /// Gets a value indicating whether the ordering should be ascending (true) or descending (false).
    /// </summary>
    public bool Ascending { get; }

    /// <summary>
    /// Gets the <see cref="System.Type"/> of the property on which to order.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Gets the <see cref="System.Linq.Expressions.Expression"/> the contains the ordering rule.
    /// </summary>
    public Expression Expression { get; }

    /// <summary>
    /// Constructor takes in bool, Type and Expression ans assign them to public properties.
    /// </summary>
    /// <param name="isAsc">boolean value for asc parameter.</param>
    /// <param name="type">Type of <see cref="OrderStep"/>.</param>
    /// <param name="expression">Instance of <see cref="Expression"/>.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "AV1564:Parameter in public or internal member is of type bool or bool?", Justification = "Using booleans provides an easy to understand parameter.")]
    public OrderStep(bool isAsc, Type type, Expression expression)
    {
        Ascending = isAsc;
        Type = type;
        Expression = expression;
    }
}