using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore;

/// <summary>
/// Exposes CRUD methods for a given Data Repository.
/// </summary>
/// <typeparam name="TContext">Type DataContext.</typeparam>
public sealed class WriteDataRepository<TContext> : IWriteableDataRepository, IDisposable
    where TContext : DbContext
{
    private readonly TContext _context;

    /// <summary>
    /// Constructor takes in an instance of TContext}.
    /// </summary>
    /// <param name="context">Instance of TContext.</param>
    public WriteDataRepository(TContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Add a given model to data context.
    /// </summary>
    /// <typeparam name="T">Type of model.</typeparam>
    /// <param name="model">Instance of the model.</param>
    /// <returns>A TContext type object.</returns>
    public T Add<T>(T model) where T : class
    {
        return _context.Add(model).Entity;
    }

    /// <summary>
    /// Add a given model to data context Asynchronously.
    /// </summary>
    /// <typeparam name="T">Type of model.</typeparam>
    /// <param name="model">Instance of the model.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A TContext type object.</returns>
    public async Task<T> AddAsync<T>(T model, CancellationToken cancellationToken = default)
        where T : class
    {
        return (await _context.AddAsync(model, cancellationToken).ConfigureAwait(false)).Entity;
    }

    /// <summary>
    /// Update the given model wiht in the data context.
    /// </summary>
    /// <typeparam name="T">Type of model.</typeparam>
    /// <param name="model">Instance of the model.</param>
    public void Update<T>(T model) where T : class
    {
        _context.Update(model);
    }

    /// <summary>
    /// Deletes the given model with in the data context.
    /// </summary>
    /// <typeparam name="T">Type of model.</typeparam>
    /// <param name="model">Instance of the model.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("naming-conventions", "AV1711:Name members similarly to members of related .NET Framework classes", Justification = "Introduce breaking changes.")]
    public void Delete<T>(T model) where T : class
    {
        _context.Remove(model);
    }

    /// <summary>
    /// Add a given model to data context Asynchronously.
    /// </summary>
    /// <typeparam name="T">Type of model.</typeparam>
    /// <param name="primaryKeys">List of primary key objects.</param>
    /// <returns>Boolean task result.</returns>
    public async Task<bool> DeleteAsync<T>(params object[] primaryKeys) where T : class
    {
        var model = await _context.FindAsync<T>(primaryKeys).ConfigureAwait(false);
        if (model == null)
        {
            return false;
        }

        Delete(model);

        return true;
    }

    /// <summary>
    /// Dispose TContext object.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP007:Don't dispose injected", Justification = "Needs this for clearing out context object.")]
    public void Dispose()
    {
        _context?.Dispose();
    }
}