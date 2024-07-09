using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Projection;

/// <summary>
/// Represents an <see cref="IProjectionSpecification{T, TResult}"/> created from an arbitrary <see cref="Expression{TDelegate}"/>}"/>.
/// </summary>
/// <typeparam name="T">The type of entity to be converted from.</typeparam>
/// <typeparam name="TResult">The type of entity to be converted to.</typeparam>
public class DynamicProjectionSpecification<T, TResult> : IProjectionSpecification<T, TResult>
{
    /// <summary>
    /// Gets Expression.
    /// </summary>
    public Expression<Func<T, TResult>> Expression { get; }

    /// <summary>
    /// Constructor takes in <see cref="Expression{T}"/>.
    /// </summary>
    /// <param name="expression">Instance of <see cref="Expression{T}"/>.</param>
    public DynamicProjectionSpecification(Expression<Func<T, TResult>> expression)
    {
        Expression = expression;
    }
}