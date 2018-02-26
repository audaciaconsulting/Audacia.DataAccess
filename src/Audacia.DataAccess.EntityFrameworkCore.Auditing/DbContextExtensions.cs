using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public static class DbContextExtensions
    {
        private static readonly ConditionalWeakTable<DbContext, AuditDbCommandListener> AuditDbCommandListenerLookup =
            new ConditionalWeakTable<DbContext, AuditDbCommandListener>();

        public static void AddAuditor(this DbContext dbContext, BlockingDbCommandAuditor auditor)
        {
            auditor.RegisterContext(dbContext);

            dbContext.InitOrGetAuditDbCommandListener().AddAuditor(auditor);
        }

        public static void AddAuditor(this DbContext dbContext, FireAndForgetDbCommandAuditor auditor)
        {
            auditor.RegisterContext(dbContext);

            dbContext.InitOrGetAuditDbCommandListener().AddAuditor(auditor);
        }

        private static AuditDbCommandListener InitOrGetAuditDbCommandListener(this DbContext dbContext)
        {
            if (!AuditDbCommandListenerLookup.TryGetValue(dbContext, out var auditDbCommandListener))
            {
                auditDbCommandListener = new AuditDbCommandListener();

                var diagnosticListener = dbContext.GetService<DiagnosticSource>() as DiagnosticListener;

                diagnosticListener.SubscribeWithAdapter(auditDbCommandListener);

                AuditDbCommandListenerLookup.Add(dbContext, auditDbCommandListener);
            }

            return auditDbCommandListener;
        }
    }
}