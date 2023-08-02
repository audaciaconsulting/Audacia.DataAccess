using Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;
using Audacia.DataAccess.EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing;

public class AuditContext<TDbContext>
    where TDbContext : DbContext
{
    public AuditContext(IEntityAuditConfiguration configuration, TriggerContext<TDbContext> triggerContext)
    {
        Configuration = configuration;
        TriggerContext = triggerContext;
    }

    public IEntityAuditConfiguration Configuration { get; }
    public TriggerContext<TDbContext> TriggerContext { get; }
}