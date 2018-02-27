using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    // ReSharper disable once UnusedTypeParameter
    public class AuditConfiguration<TDbContext>
        where TDbContext : DbContext
    {
        internal AuditConfiguration()
        {
        }

        public bool DoNotAuditIfNoChangesInTrackedProperties { get; internal set; }
        public AuditStrategy Strategy { get; internal set; }

        public IDictionary<IEntityType, EntityAuditConfiguration> Entities { get; internal set; }
    }
}