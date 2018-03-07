using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    public class AuditConfiguration<TDbContext> : IAuditConfiguration<TDbContext> where TDbContext : DbContext
    {
        internal AuditConfiguration()
        {
        }

        public bool DoNotAuditIfNoChangesInTrackedProperties { get; internal set; }
        public AuditStrategy Strategy { get; internal set; }

        public IDictionary<Type, IEntityAuditConfiguration> Entities { get; internal set; }
    }
}