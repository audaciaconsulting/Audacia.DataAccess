using Audacia.Core.Extensions;
using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Filtering;

/// <summary>
/// Combines two or more <see cref="IFilterSpecification{T}"/> objects into a single <see cref="IFilterSpecification{T}"/>
/// using the logical AND operator.
/// </summary>
/// <typeparam name="T">The type of entity to be filtered.</typeparam>
public class AndFilterSpecification<T> : IFilterSpecification<T>
{
    public Expression<Func<T, bool>> Expression { get; }

    public AndFilterSpecification(IFilterSpecification<T> left, IFilterSpecification<T> right)
    {
        Expression = left.Expression.And(right.Expression);
    }

    public AndFilterSpecification(IFilterSpecification<T> left, Expression<Func<T, bool>> right)
    {
        Expression = left.Expression.And(right);
    }

    public AndFilterSpecification(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right, params Expression<Func<T, bool>>[] additionalExpressions)
    {
        Expression = left.And(right);
        foreach (var expression in additionalExpressions)
        {
            Expression = Expression.And(expression);
        }
    }
}