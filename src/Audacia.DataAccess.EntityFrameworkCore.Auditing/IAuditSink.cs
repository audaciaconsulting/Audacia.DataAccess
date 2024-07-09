using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing;

/// <summary>
/// IAuditSink interface.
/// </summary>
/// <typeparam name="TUserIdentifier">UserIdentifier struct stype.</typeparam>
public interface IAuditSink<TUserIdentifier>
    where TUserIdentifier : struct
{
    /// <summary>
    /// HandleAsync method.
    /// </summary>
    /// <param name="auditEntries">Collection of auditEntries. </param>
    /// <param name="cancellationToken">CancellationToken.</param>
    /// <returns>Task.</returns>
    Task HandleAsync(IEnumerable<AuditEntry<TUserIdentifier>> auditEntries, CancellationToken cancellationToken);
}