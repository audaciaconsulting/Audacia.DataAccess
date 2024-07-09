using Audacia.Core;
using Audacia.DataAccess.Specifications.Projection;

namespace Audacia.DataAccess.Specifications.Paging.Sorting;

/// <summary>
/// Allows a collection of objects of type <see cref="IQuerySpecification{T}"/> to be filtered, paged and have
/// navigation properties included by exposing an <see cref="SortablePagingRequest"/> 
/// in addition to the functionality provided by a <see cref="IQuerySpecification{T}"/>.
/// </summary>
/// <typeparam name="T">Type <see cref="IQuerySpecification{T}"/>.</typeparam>
public interface ISortablePageableQuerySpecification<T> : IQuerySpecification<T> where T : class
{
    /// <summary>
    /// Gets or sets SortablePagingRequest.
    /// </summary>
    SortablePagingRequest SortablePagingRequest { get; set; }
}

/// <summary>
/// Allows a collection of objects of type <see cref="IProjectableQuerySpecification{T,TResult}"/> to be filtered, paged,
/// converted from an object of type <see cref="IProjectableQuerySpecification{T,TResult}"/> to an object of type <see cref="ISortablePageableQuerySpecification{T}"/>
/// and have  navigation properties included by exposing an <see cref="SortablePagingRequest"/> 
/// in addition to the functionality provided by a <see cref="IProjectableQuerySpecification{T,TResult}"/>.
/// </summary>
/// <typeparam name="T">Type of <see cref="ISortablePageableQuerySpecification{T}"/>.</typeparam>
/// <typeparam name="TResult">Return type of <see cref="ISortablePageableQuerySpecification{TResult}"/>. </typeparam>
public interface ISortablePageableQuerySpecification<T, TResult> : IProjectableQuerySpecification<T, TResult> where T : class
{
    /// <summary>
    /// Gets or sets SortablePagingRequest.
    /// </summary>
    SortablePagingRequest SortablePagingRequest { get; set; }
}