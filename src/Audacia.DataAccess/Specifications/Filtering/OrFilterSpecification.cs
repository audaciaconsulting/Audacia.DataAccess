using Audacia.Core.Extensions;
using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Filtering
{
    /// <summary>
    /// Combines two or more <see cref="IFilterSpecification{T}"/> objects into a single <see cref="IFilterSpecification{T}"/>
    /// using the logical OR operator.
    /// </summary>
    /// <typeparam name="T">The type of entity to be filtered.</typeparam>
    public class OrFilterSpecification<T> : IFilterSpecification<T>
    {
        public Expression<Func<T, bool>> Expression { get; }

        public OrFilterSpecification(IFilterSpecification<T> left, IFilterSpecification<T> right)
        {
            Expression = left.Expression.Or(right.Expression);
        }

        public OrFilterSpecification(IFilterSpecification<T> left, Expression<Func<T, bool>> right)
        {
            Expression = left.Expression.Or(right);
        }

        public OrFilterSpecification(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right, params Expression<Func<T, bool>>[] additionalExpressions)
        {
            Expression = left.Or(right);
            foreach (var expression in additionalExpressions)
            {
                Expression = Expression.Or(expression);
            }
        }
    }
}