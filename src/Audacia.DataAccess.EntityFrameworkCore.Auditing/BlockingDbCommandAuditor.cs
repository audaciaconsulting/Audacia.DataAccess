using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public abstract class BlockingDbCommandAuditor
    {
        public abstract void OnCommandExecuting(DbCommand command, DbCommandMethod executeMethod, Guid commandId,
            Guid connectionId, bool async, DateTimeOffset startTime);

        public abstract void OnCommandExecuted(object result, bool async);
        
        internal void RegisterContext(DbContext context)
        {
            Context = context;
        }

        protected DbContext Context { get; private set; }
    }
}