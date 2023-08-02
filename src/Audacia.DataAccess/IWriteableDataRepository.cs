using System.Threading;
using System.Threading.Tasks;

namespace Audacia.DataAccess;

/// <summary>
/// Exposes the methods to add and remove model instances from the underlying data storage.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "AV1554:Method contains optional parameter in type hierarchy", Justification = "Allows to include an existing cancellation token when invoking methods. Otherwise, a new token is provided.")]
public interface IWriteableDataRepository
{
    /// <summary>
    /// Adds the given <paramref name="model"/> to the data repository.
    /// </summary>
    /// <typeparam name="T">Elemety type of the model.</typeparam>
    /// <param name="model">The entity to add.</param>
    /// <returns>The entity.</returns>
    T Add<T>(T model) where T : class;

    /// <summary>
    /// Adds the given <paramref name="model"/> to the data repository asynchronously.
    /// </summary>
    /// <typeparam name="T">Elemety type of the model.</typeparam>
    /// <param name="model">The entity to add.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>The entity.</returns>
    Task<T> AddAsync<T>(T model, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Updates the given <paramref name="model"/> in the data repository.
    /// </summary>
    /// <typeparam name="T">Elemety type of the model.</typeparam>
    /// <param name="model">The entity to update.</param>
    void Update<T>(T model) where T : class;

    /// <summary>
    /// Deletes the given <paramref name="model"/> from the data repository.
    /// </summary>
    /// <typeparam name="T">Elemety type of the model.</typeparam>
    /// <param name="model">The entity to delete.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "AV1711:Name members and local functions similarly to members of .NET Framework classes", Justification = "Method name is consistant with EF DbSet method name.")]
    void Delete<T>(T model) where T : class;

    /// <summary>
    /// Deletes the model instance identified by the given <paramref name="primaryKeys"/> from the data repository.
    /// </summary>
    /// <typeparam name="T">Elemety type of the model.</typeparam>
    /// <param name="primaryKeys">The primary keys of entities to delete.</param>
    /// <returns>A <see cref="Task{TResult}"/> that, when complete, contains a <see cref="bool"/> value describing whether or not the deletion was successful.</returns>
    Task<bool> DeleteAsync<T>(params object[] primaryKeys) where T : class;
}