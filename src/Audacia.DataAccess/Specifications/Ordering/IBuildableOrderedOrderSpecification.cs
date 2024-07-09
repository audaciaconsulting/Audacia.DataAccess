using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Ordering;

/// <summary>
/// Exposes methods that allow an <see cref="IOrderSpecification{T}"/> to be incrementally built from <see cref="Expression{TDelegate}"/>s.
/// </summary>
/// <typeparam name="T">Type of <see cref="IBuildableOrderedOrderSpecification{TKey}"/>.</typeparam>
public interface IBuildableOrderedOrderSpecification<T> : IOrderSpecification<T>
{
    /// <summary>
    /// Specifies to sort the collection of objects of type <see cref="IBuildableOrderedOrderSpecification{T}"/> in ascending order 
    /// by a column of type <see cref="IBuildableOrderedOrderSpecification{TKey}"/> using the rule given by the <paramref name="keySelector"/>
    /// as a subsequent ordering step.
    /// </summary>
    /// <typeparam name="TKey">Return type of <see cref="IBuildableOrderedOrderSpecification{TKey}"/>.</typeparam>
    /// <param name="keySelector">Instance of <see cref="Expression{T}"/>.</param>
    /// <returns><see cref="IBuildableOrderedOrderSpecification{TKey}"/>.</returns>
    IBuildableOrderedOrderSpecification<T> ThenAsc<TKey>(Expression<Func<T, TKey>> keySelector);

    /// <summary>
    /// Specifies to sort the collection of objects of type <see cref="IBuildableOrderedOrderSpecification{T}"/> in descending order 
    /// by a column of type <see cref="IBuildableOrderedOrderSpecification{TKey}"/> using the rule given by the <paramref name="keySelector"/>
    /// as a subsequent ordering step.
    /// </summary>
    /// <typeparam name="TKey">Return type of <see cref="IBuildableOrderedOrderSpecification{TKey}"/>.</typeparam>
    /// <param name="keySelector">Instance of <see cref="Expression{T}"/>.</param>
    /// <returns><see cref="IBuildableOrderedOrderSpecification{TKey}"/>.</returns>
    IBuildableOrderedOrderSpecification<T> ThenDesc<TKey>(Expression<Func<T, TKey>> keySelector);
}