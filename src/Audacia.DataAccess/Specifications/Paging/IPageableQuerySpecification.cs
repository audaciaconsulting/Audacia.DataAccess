using Audacia.Core;
using Audacia.DataAccess.Specifications.Ordering;
using Audacia.DataAccess.Specifications.Projection;

namespace Audacia.DataAccess.Specifications.Paging
{
    /// <summary>
    /// Allows a collection of objects of type <see cref="T"/> to be filtered, paged and have
    /// navigation properties included by exposing an <see cref="PagingRequest"/> 
    /// in addition to the functionality provided by a <see cref="IOrderableQuerySpecification{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPageableQuerySpecification<T> : IOrderableQuerySpecification<T> where T : class
    {
        PagingRequest PagingRequest { get; set; }
    }

    /// <summary>
    /// Allows a collection of objects of type <see cref="TResult"/> to be filtered, paged,
    /// converted from an object of type <see cref="T"/> to an object of type <see cref="TResult"/>
    /// and have  navigation properties included by exposing an <see cref="PagingRequest"/> 
    /// in addition to the functionality provided by a <see cref="IProjectableQuerySpecification{T,TResult}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IPageableQuerySpecification<T, TResult> :
        IOrderableQuerySpecification<T, TResult>
        where T : class
    {
        PagingRequest PagingRequest { get; set; }
    }
}