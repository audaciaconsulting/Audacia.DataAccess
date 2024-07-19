using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Filtering;

/// <summary>
/// Exposes an expression to filter from a collection of objects of type.
/// </summary>
/// <typeparam name="T">The type of entity to be filtered.</typeparam>
public interface IFilterSpecification<T>
{
    /// <summary>
    /// Gets the <see cref="Expression{TDelegate}"/> that contains the filtering rule(s).
    /// </summary>
    Expression<Func<T, bool>> Expression { get; }
}