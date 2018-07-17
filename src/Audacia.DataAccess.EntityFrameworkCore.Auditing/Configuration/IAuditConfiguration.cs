using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    public interface IAuditConfiguration<TUserIdentifier, in TDbContext>
        where TDbContext : DbContext
        where TUserIdentifier : struct
    {
        bool DoNotAuditIfNoChangesInTrackedProperties { get; }
        AuditStrategy Strategy { get; }
        IDictionary<Type, IEntityAuditConfiguration> Entities { get; }
        Func<TUserIdentifier?> UserIdentifierFactory { get; }
        IEnumerable<IAuditSinkFactory<TUserIdentifier, TDbContext>> SinkFactories { get; }
    }
}