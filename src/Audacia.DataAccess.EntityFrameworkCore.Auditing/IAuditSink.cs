using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public interface IAuditSink<TUserIdentifier>
        where TUserIdentifier : struct
    {
        Task HandleAsync(IEnumerable<AuditEntry<TUserIdentifier>> auditEntries, CancellationToken cancellationToken);
    }
}