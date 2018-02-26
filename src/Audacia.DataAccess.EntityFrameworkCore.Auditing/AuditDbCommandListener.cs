using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DiagnosticAdapter;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    //https://weblogs.asp.net/ricardoperes/interception-in-entity-framework-core
    public sealed class AuditDbCommandListener
    {
        private readonly ICollection<FireAndForgetDbCommandAuditor> _fireAndForgetAuditors =
            new List<FireAndForgetDbCommandAuditor>();

        private readonly ICollection<BlockingDbCommandAuditor> _blockingAuditors =
            new List<BlockingDbCommandAuditor>();

        public AuditDbCommandListener AddAuditor(FireAndForgetDbCommandAuditor auditor)
        {
            _fireAndForgetAuditors.Add(auditor);

            return this;
        }

        public AuditDbCommandListener AddAuditor(BlockingDbCommandAuditor auditor)
        {
            _blockingAuditors.Add(auditor);

            return this;
        }

        [DiagnosticName("Microsoft.EntityFrameworkCore.Database.Command.CommandExecuting")]
        public void OnCommandExecuting(DbCommand command, DbCommandMethod executeMethod, Guid commandId,
            Guid connectionId, bool async, DateTimeOffset startTime)
        {
            foreach (var fireAndForgetDbCommandAuditor in _fireAndForgetAuditors)
            {
                Task.Run(async () =>
                    await fireAndForgetDbCommandAuditor.OnCommandExecutingAsync(command, executeMethod, commandId,
                        connectionId, async, startTime));
            }

            foreach (var blockingDbCommandAuditor in _blockingAuditors)
            {
                blockingDbCommandAuditor.OnCommandExecuting(command, executeMethod, commandId, connectionId, async,
                    startTime);
            }
        }

        [DiagnosticName("Microsoft.EntityFrameworkCore.Database.Command.CommandExecuted")]
        public void OnCommandExecuted(object result, bool async)
        {
            foreach (var fireAndForgetDbCommandAuditor in _fireAndForgetAuditors)
            {
                Task.Run(async () =>
                    await fireAndForgetDbCommandAuditor.OnCommandExecutedAsync(result, async));
            }

            foreach (var blockingDbCommandAuditor in _blockingAuditors)
            {
                blockingDbCommandAuditor.OnCommandExecuted(result, async);
            }
        }
    }
}