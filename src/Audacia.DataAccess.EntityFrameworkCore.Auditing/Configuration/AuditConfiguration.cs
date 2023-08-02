using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

internal class AuditConfiguration<TUserIdentifier, TDbContext> : IAuditConfiguration<TUserIdentifier, TDbContext>
    where TDbContext : DbContext
    where TUserIdentifier : struct
{
    internal AuditConfiguration()
    {
    }

    public bool DoNotAuditIfNoChangesInTrackedProperties { get; internal set; }
    public AuditStrategy Strategy { get; internal set; }

    public IDictionary<Type, IEntityAuditConfiguration> Entities { get; internal set; }
    public Func<TUserIdentifier?> UserIdentifierFactory { get; internal set; }
    public IEnumerable<IAuditSinkFactory<TUserIdentifier, TDbContext>> SinkFactories { get; set; }
}