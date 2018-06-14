using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public abstract class TransactionalAuditSink<TDbContext> : IAuditSink
        where TDbContext : DbContext
    {
        internal void SetContext(TDbContext context)
        {
            Context = context;
        }

        protected TDbContext Context { get; private set; }

        public abstract Task HandleAsync(IEnumerable<AuditEntry> auditEntries, CancellationToken cancellationToken);
    }
}