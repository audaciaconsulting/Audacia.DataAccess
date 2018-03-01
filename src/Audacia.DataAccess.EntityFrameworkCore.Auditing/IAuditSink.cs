using System.Collections.Generic;
using System.Threading.Tasks;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public interface IAuditSink
    {
        Task HandleAsync(IEnumerable<AuditEntry> auditEntries);
    }
}