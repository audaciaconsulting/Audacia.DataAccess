using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    // ReSharper disable once UnusedTypeParameter
    public interface IAuditConfiguration<TDbContext>
        where TDbContext : DbContext
    {
        bool DoNotAuditIfNoChangesInTrackedProperties { get; }
        AuditStrategy Strategy { get; }
        IDictionary<IEntityType, IEntityAuditConfiguration> Entities { get; }
    }
}