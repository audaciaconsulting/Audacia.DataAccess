using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Projection
{
    /// <summary>
    /// Exposes an expression to convert from an object of type <see cref="T"/> to an object of type <see cref="TResult"/>.
    /// </summary>
    /// <typeparam name="T">The type of entity to be converted from.</typeparam>
    /// <typeparam name="TResult">The type of entity to be converted to.</typeparam>
    public interface IProjectionSpecification<T, TResult>
    {
        /// <summary>
        /// Get the <see cref="Expression{TDelegate}"/> that contains the projection rules.
        /// </summary>
        /// <returns></returns>
        Expression<Func<T, TResult>> Expression { get; }
    }
}