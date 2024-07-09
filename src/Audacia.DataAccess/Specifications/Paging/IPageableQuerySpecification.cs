using Audacia.Core;
using Audacia.DataAccess.Specifications.Ordering;

namespace Audacia.DataAccess.Specifications.Paging;

/// <summary>
/// Allows a collection of objects of type <see cref="IOrderableQuerySpecification{T}"/> to be filtered, paged and have
/// navigation properties included by exposing an <see cref="PagingRequest"/> 
/// in addition to the functionality provided by a <see cref="IOrderableQuerySpecification{T}"/>.
/// </summary>
/// <typeparam name="T">Type of <see cref="IOrderableQuerySpecification{T}"/>.</typeparam>
public interface IPageableQuerySpecification<T> : IOrderableQuerySpecification<T> where T : class
{
    /// <summary>
    /// Gets or sets PagingRequest.
    /// </summary>
    PagingRequest PagingRequest { get; set; }
}

/// <summary>
/// Allows a collection of objects of type <see cref="IOrderableQuerySpecification{TResult}"/> to be filtered, paged,
/// converted from an object of type <see cref="IOrderableQuerySpecification{T}"/> to an object of type <see cref="IOrderableQuerySpecification{T}"/>
/// and have  navigation properties included by exposing an <see cref="PagingRequest"/> 
/// in addition to the functionality provided by a <see cref="IOrderableQuerySpecification{T,TResult}"/>.
/// </summary>
/// <typeparam name="T">Type of <see cref="IPageableQuerySpecification{T}"/>.</typeparam>
/// <typeparam name="TResult">Return type of <see cref="IPageableQuerySpecification{TResult}"/>.</typeparam>
public interface IPageableQuerySpecification<T, TResult> :
    IOrderableQuerySpecification<T, TResult>
    where T : class
{
    /// <summary>
    /// Gets or sets PagingRequest.
    /// </summary>
    PagingRequest PagingRequest { get; set; }
}