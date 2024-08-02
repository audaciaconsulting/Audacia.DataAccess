using System;
using System.Linq.Expressions;
using Audacia.Core.Extensions;

namespace Audacia.DataAccess.Specifications.Filtering;

/// <summary>
/// Combines two or more <see cref="IFilterSpecification{T}"/> objects into a single <see cref="IFilterSpecification{T}"/>
/// using the logical OR operator.
/// </summary>
/// <typeparam name="T">The type of entity to be filtered.</typeparam>
public class OrFilterSpecification<T> : IFilterSpecification<T>
{
    /// <summary>
    /// Gets the <see cref="Expression{T}"/> that contains the filtering rule(s).
    /// </summary>
    public Expression<Func<T, bool>> Expression { get; }

    /// <summary>
    /// Constructor which takes in two instances of <see cref="IFilterSpecification{T}"/>.
    /// </summary>
    /// <param name="left">Left part of the Expression <see cref="IFilterSpecification{T}"/>.</param>
    /// <param name="right">Right part of the Expression <see cref="IFilterSpecification{T}"/>.</param>
    public OrFilterSpecification(IFilterSpecification<T> left, IFilterSpecification<T> right)
    {
        if (left is null)
        {
            throw new ArgumentNullException(nameof(left));
        }

        if (right is null)
        {
            throw new ArgumentNullException(nameof(right));
        }

        Expression = left.Expression.Or(right.Expression);
    }

    /// <summary>
    /// Constructor which takes an instance of <see cref="IFilterSpecification{T}"/> and an instance of  <see cref="Expression{T}"/>.
    /// </summary>
    /// <param name="left">Left part of the Expression <see cref="IFilterSpecification{T}"/>.</param>
    /// <param name="right">Right part of the Expression <see cref="Expression{T}"/>.</param>
    public OrFilterSpecification(IFilterSpecification<T> left, Expression<Func<T, bool>> right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        Expression = left.Expression.Or(right);
    }

    /// <summary>
    /// Constructor which takes an instance of <see cref="IFilterSpecification{T}"/> , 
    /// an instance of  <see cref="Expression"/> and an instance of <see cref="Expression"/> as additional expressions.
    /// </summary>
    /// <param name="left">Left part of the Expression <see cref="Expression{T}"/>.</param>
    /// <param name="right">Right part of the Expression <see cref="Expression{T}"/>.</param>
    /// <param name="additionalExpressions">Additional expressions <see cref="Expression{T}"/>.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Spacing Rules", "SA1010:Opening Square Brackets Must Be Spaced Correctly", Justification = "This is the only way to create an empty array.")]
    public OrFilterSpecification(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right, params Expression<Func<T, bool>>[] additionalExpressions)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        Expression = left.Or(right);
        foreach (var expression in additionalExpressions ?? [])
        {
            Expression = Expression.Or(expression);
        }
    }
}