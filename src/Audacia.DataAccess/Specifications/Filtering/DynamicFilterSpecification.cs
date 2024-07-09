using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Filtering;

/// <summary>
/// Represents an <see cref="IFilterSpecification{T}"/> created from an arbitrary <see cref="Expression{TDelegate}"/>}"/>.
/// </summary>
/// <typeparam name="T">The type of entity to be filtered.</typeparam>
public class DynamicFilterSpecification<T> : IFilterSpecification<T>
{
    /// <summary>
    /// Gets the <see cref="Expression{T}"/> that contains the filtering rule(s).
    /// </summary>
    public Expression<Func<T, bool>> Expression { get; }

    /// <summary>
    /// Constructor which takes an instance of <see cref="Expression{TDelegate}"/>.
    /// </summary>
    /// <param name="expression">Instance of <see cref="Expression{TDelegate}"/>.</param>
    public DynamicFilterSpecification(Expression<Func<T, bool>> expression)
    {
        Expression = expression;
    }
}