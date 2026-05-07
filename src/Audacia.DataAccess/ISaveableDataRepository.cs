using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Audacia.DataAccess;

/// <summary>
/// Exposes the methods to allow changes made to tracked instances of the model to be persisted to the underlying data storage mechanism.
/// </summary>
[SuppressMessage("Maintainability", "AV1554:Method contains optional parameter in type hierarchy", Justification = "Allows to include an existing cancellation token when invoking methods. Otherwise, a new token is provided.")]
public interface ISaveableDataRepository
{
    /// <summary>
    /// Asynchronously persists tracked changes of a model to the underlying data storage mechanism.
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess">Defines if all changes will be accepted after saving.</param>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>The number of state entries written to the underlying database.</returns>
    [SuppressMessage("Maintainability", "ACL1017:Parameter in public or internal member is of type bool or bool?", Justification = "Changing this will introduce breaking changes.")]
    Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously persists tracked changes of a model to the underlying data storage mechanism.
    /// </summary>
    /// <param name="cancellationToken">A token for cancelling asynchronous tasks.</param>
    /// <returns>The number of state entries written to the underlying database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}