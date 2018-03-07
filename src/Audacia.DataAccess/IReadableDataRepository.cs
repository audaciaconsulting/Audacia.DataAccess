using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Audacia.Core;
using Audacia.DataAccess.Specifications;
using Audacia.DataAccess.Specifications.DataStoreImplementations;
using Audacia.DataAccess.Specifications.Ordering;
using Audacia.DataAccess.Specifications.Paging;
using Audacia.DataAccess.Specifications.Paging.Sorting;
using Audacia.DataAccess.Specifications.Projection;

namespace Audacia.DataAccess
{
    /// <summary>
    /// Exposes the methods to allow the underlying data storage to be queried.
    /// The query rules are encapsulated in the specification objects passed into each method as a parameter.
    /// </summary>
    public interface IReadableDataRepository
    {
        /// <summary>
        /// Asynchronously determines whether all the elements of type <see cref="T"/> satisfy the rules in the given <paramref name="specification"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> AllAsync<T>(IQuerySpecification<T> specification,
            CancellationToken cancellationToken = new CancellationToken()) where T : class;

        /// <summary>
        /// Asynchronously determines whether any elements of type <see cref="T"/> satisfy the rules in the given <paramref name="specification"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> AnyAsync<T>(IQuerySpecification<T> specification,
            CancellationToken cancellationToken = new CancellationToken()) where T : class;
        
        /// <summary>
        /// Gets the first instance of the model of type <see cref="T"/> that matches the rules in the given <paramref name="specification"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(IOrderableQuerySpecification<T> specification,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class;

        /// <summary>
        /// Gets all instances of the model of type <see cref="T"/> that match the rules in the given <paramref name="specification"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync<T>(IOrderableQuerySpecification<T> specification,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class;

        /// <summary>
        /// Gets the first instance of the model of type <see cref="T"/>, projected to an object of type <see cref="TResult"/>,
        /// that matches the rules in the given <paramref name="specification"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TResult> GetAsync<T, TResult>(IProjectableQuerySpecification<T, TResult> specification,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class;

        /// <summary>
        /// Gets all instances of the model of type <see cref="T"/>, projected to an object of type <see cref="TResult"/>,
        /// that match the rules in the given <paramref name="specification"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> GetAllAsync<T, TResult>(IProjectableQuerySpecification<T, TResult> specification,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class;
        
        /// <summary>
        /// Gets the first instance of the model of type <see cref="T"/>, projected to an object of type <see cref="TResult"/>,
        /// that matches the rules in the given <paramref name="specification"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TResult> GetAsync<T, TResult>(IOrderableQuerySpecification<T, TResult> specification,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class;

        /// <summary>
        /// Gets all instances of the model of type <see cref="T"/>, projected to an object of type <see cref="TResult"/>,
        /// that match the rules in the given <paramref name="specification"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> GetAllAsync<T, TResult>(IOrderableQuerySpecification<T, TResult> specification,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class;
        
        /// <summary>
        /// Gets a page of the model of type <see cref="T"/> that match the rules in the given <paramref name="specification"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPage<T>> GetPageAsync<T>(IPageableQuerySpecification<T> specification,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class;

        /// <summary>
        /// Gets a page of the model of type <see cref="T"/>, projected to an object of type <see cref="TResult"/>, that match the rules in the given <paramref name="specification"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPage<TResult>> GetPageAsync<T, TResult>(IPageableQuerySpecification<T, TResult> specification,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class;

        /// <summary>
        /// Gets a page of the model of type <see cref="T"/> that match the rules in the given <paramref name="specification"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPage<T>> GetPageAsync<T>(ISortablePageableQuerySpecification<T> specification,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class;

        /// <summary>
        /// Gets a page of the model of type <see cref="T"/>, projected to an object of type <see cref="TResult"/>, that match the rules in the given <paramref name="specification"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPage<TResult>> GetPageAsync<T, TResult>(ISortablePageableQuerySpecification<T, TResult> specification,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class;

        /// <summary>
        /// Gets all instances of the model of type <see cref="T"/> that match the rules in the given <paramref name="specification"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync<T>(IDataStoreImplementedQuerySpecification<T> specification,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class;

        /// <summary>
        /// Gets all instances of the model of type <see cref="T"/>, projected to an object of type <see cref="TResult"/>,
        /// that match the rules in the given <paramref name="specification"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> GetAllAsync<T, TResult>(
            IDataStoreImplementedQuerySpecification<T, TResult> specification,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class;
    }
}