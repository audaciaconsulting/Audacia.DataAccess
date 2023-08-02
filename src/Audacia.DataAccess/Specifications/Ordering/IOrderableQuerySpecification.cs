using Audacia.DataAccess.Specifications.Projection;

namespace Audacia.DataAccess.Specifications.Ordering;

/// <summary>
/// Allows a collection of objects of type <see cref="T"/> to be filtered, sorted and have
/// navigation properties included by exposing an <see cref="IOrderSpecification{T}"/> 
/// in addition to the functionality provided by a <see cref="IQuerySpecification{T}"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IOrderableQuerySpecification<T> : IQuerySpecification<T>
{
    /// <summary>
    /// Gets an <see cref="IOrderSpecification{T}"/> contains the sorting rules.
    /// </summary>
    IOrderSpecification<T> Order { get; set; }
}

/// <summary>
/// Allows a collection of objects of type <see cref="TResult"/> to be filtered, sorted,
/// converted from an object of type <see cref="T"/> to an object of type <see cref="TResult"/>
/// and have  navigation properties included by exposing an <see cref="IOrderSpecification{T}"/> 
/// in addition to the functionality provided by a <see cref="IProjectableQuerySpecification{T,TResult}"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IOrderableQuerySpecification<T, TResult> : IProjectableQuerySpecification<T, TResult> where T : class
{
    /// <summary>
    /// Gets an <see cref="IOrderSpecification{TResult}"/> contains the sorting rules.
    /// </summary>
    IOrderSpecification<TResult> Order { get; set; }
}