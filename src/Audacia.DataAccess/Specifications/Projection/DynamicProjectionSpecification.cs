using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Projection
{
    /// <summary>
    /// Represents an <see cref="IProjectionSpecification{T, TResult}"/> created from an arbitrary <see cref="Expression{TDelegate}"/>}"/>.
    /// </summary>
    /// <typeparam name="T">The type of entity to be converted from.</typeparam>
    /// <typeparam name="TResult">The type of entity to be converted to.</typeparam>
    public class DynamicProjectionSpecification<T, TResult> : IProjectionSpecification<T, TResult>
    {
        public Expression<Func<T, TResult>> Expression { get; }

        public DynamicProjectionSpecification(Expression<Func<T, TResult>> expression)
        {
            Expression = expression;
        }
    }
}