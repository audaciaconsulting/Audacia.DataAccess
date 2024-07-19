using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Ordering;

/// <summary>
/// Exposes methods that allow an <see cref="IOrderSpecification{T}"/> to be incrementally built from <see cref="Expression{TDelegate}"/>s.
/// </summary>
/// <typeparam name="T">Type of <see cref="IBuildableOrderSpecification{T}"/>.</typeparam>
public interface IBuildableOrderSpecification<T> : IOrderSpecification<T>
{
    /// <summary>
    /// Specifies to sort the collection of objects of type <see cref="IBuildableOrderedOrderSpecification{T}"/> in ascending order 
    /// by a column of type  <see cref="IBuildableOrderedOrderSpecification{TKey}"/> using the rule given by the <paramref name="keySelector"/>.
    /// </summary>
    /// <typeparam name="TKey">Return type of <see cref="IBuildableOrderedOrderSpecification{TKey}"/>.</typeparam>
    /// <param name="keySelector">Instance of <see cref="Expression{T}"/>.</param>
    /// <returns><see cref="IBuildableOrderedOrderSpecification{T}"/>.</returns>
    IBuildableOrderedOrderSpecification<T> Asc<TKey>(Expression<Func<T, TKey>> keySelector);

    /// <summary>
    /// Specifies to sort the collection of objects of type <see cref="IBuildableOrderedOrderSpecification{T}"/> in descending order 
    /// by a column of type <see cref="IBuildableOrderedOrderSpecification{TKey}"/> using the rule given by the <paramref name="keySelector"/>.
    /// </summary>
    /// <typeparam name="TKey">Return type of <see cref="IBuildableOrderedOrderSpecification{TKey}"/>.</typeparam>
    /// <param name="keySelector">Instance of <see cref="Expression{T}"/>.</param>
    /// <returns><see cref="IBuildableOrderedOrderSpecification{T}"/>.</returns>
    IBuildableOrderedOrderSpecification<T> Desc<TKey>(Expression<Func<T, TKey>> keySelector);
}