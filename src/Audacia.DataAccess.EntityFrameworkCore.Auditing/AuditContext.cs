using Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;
using Audacia.DataAccess.EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public class AuditContext<TDbContext>
        where TDbContext : DbContext
    {
        public IEntityAuditConfiguration Configuration { get; set; }
        public TriggerContext<TDbContext> TriggerContext { get; set; }
    }
}