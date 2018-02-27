﻿using Microsoft.EntityFrameworkCore.Metadata;
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

        public IDictionary<IEntityType, IEntityAuditConfiguration> Entities { get; internal set; }
    }
}