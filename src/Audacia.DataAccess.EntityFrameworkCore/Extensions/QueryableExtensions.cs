using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Audacia.Core;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Extensions;

/// <summary>
/// Extensions for the <see cref="IQueryable{T}"/> type.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Return a page of information based on the provided <paramref name="pagingRequest"/>.
    /// </summary>
    /// <param name="query">Our query against the result set.</param>
    /// <param name="pagingRequest">Contains information required for paging and sorting.</param>
    /// <param name="cancellationToken">For cancelling asynchronous tasks, passed into EF enumeration methods.</param>
    /// <typeparam name="T">The type of each row of data on the page.</typeparam>
    /// <returns>A <see cref="Page{T}"/> of results.</returns>
    public static async Task<IPage<T>> ToPageAsync<T>(this IQueryable<T> query, PagingRequest pagingRequest,
        CancellationToken cancellationToken = default)
    {
        var totalRecords = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        var specification = new PagingSpecification<T>(query)
            .ConfigurePaging(pagingRequest)
            .UsePaging();

        var data = await specification.Query.ToListAsync(cancellationToken).ConfigureAwait(false);
        var totalPages = specification.GetTotalPages(totalRecords);

        return new Page<T>(data, totalPages, totalRecords);
    }

    /// <summary>
    /// Return a page of information, with sorting applied, based on the provided <paramref name="pagingRequest"/>.
    /// </summary>
    /// <param name="query">Our query against the result set.</param>
    /// <param name="pagingRequest">Contains information required for paging and sorting.</param>
    /// <param name="cancellationToken">For cancelling asynchronous tasks, passed into EF enumeration methods.</param>
    /// <typeparam name="T">The type of each row of data on the page.</typeparam>
    /// <returns>A <see cref="Page{T}"/> of results.</returns>
    public static async Task<IPage<T>> ToPageAsync<T>(this IQueryable<T> query, SortablePagingRequest pagingRequest,
        CancellationToken cancellationToken = default)
    {
        var totalRecords = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        var specification = new PagingSpecification<T>(query)
            .ConfigurePaging(pagingRequest)
            .ConfigureSorting(pagingRequest)
            .UseSorting()
            .UsePaging();

        var data = await specification.Query.ToListAsync(cancellationToken).ConfigureAwait(false);
        var totalPages = specification.GetTotalPages(totalRecords);

        return new Page<T>(data, totalPages, totalRecords);
    }
}