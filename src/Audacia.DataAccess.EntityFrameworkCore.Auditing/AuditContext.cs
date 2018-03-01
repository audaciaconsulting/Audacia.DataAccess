using Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;
using Audacia.DataAccess.EntityFrameworkCore.Triggers;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public class AuditContext
    {
        public IEntityAuditConfiguration Configuration { get; set; }
        public TriggerContext TriggerContext { get; set; }
    }
}