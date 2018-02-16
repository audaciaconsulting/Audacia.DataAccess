using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Filtering
{
    /// <summary>
    /// Represents an <see cref="IFilterSpecification{T}"/> created from an arbitrary <see cref="Expression{TDelegate}"/>}"/>.
    /// </summary>
    /// <typeparam name="T">The type of entity to be filtered.</typeparam>
    public class DynamicFilterSpecification<T> : IFilterSpecification<T>
    {
        public Expression<Func<T, bool>> Expression { get; }

        public DynamicFilterSpecification(Expression<Func<T, bool>> expression)
        {
            Expression = expression;
        }
    }
}