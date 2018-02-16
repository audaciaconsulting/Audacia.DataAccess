using Audacia.Core.Extensions;
using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Filtering
{
    /// <summary>
    /// Negates an <see cref="IFilterSpecification{T}"/> into a new <see cref="IFilterSpecification{T}"/> object.
    /// </summary>
    /// <typeparam name="T">The type of entity to be filtered.</typeparam>
    public class NotFilterSpecification<T> : IFilterSpecification<T>
    {
        public Expression<Func<T, bool>> Expression { get; }

        public NotFilterSpecification(IFilterSpecification<T> specificationToNegate)
        {
            Expression = specificationToNegate.Expression.Not();
        }
    }
}