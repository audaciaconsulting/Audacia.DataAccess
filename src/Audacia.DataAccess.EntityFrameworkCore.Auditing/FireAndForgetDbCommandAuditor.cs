using System;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public abstract class FireAndForgetDbCommandAuditor
    {
        public abstract Task OnCommandExecutingAsync(DbCommand command, DbCommandMethod executeMethod, Guid commandId,
            Guid connectionId, bool async, DateTimeOffset startTime);

        public abstract Task OnCommandExecutedAsync(object result, bool async);

        internal void RegisterContext(DbContext context)
        {
            Context = context;
        }

        protected DbContext Context { get; private set; }
    }
}