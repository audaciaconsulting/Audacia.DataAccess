using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

/// <summary>
/// AuditConfiguration class.
/// </summary>
/// <typeparam name="TUserIdentifier">UserIdentifier struct stype.</typeparam>
/// <typeparam name="TDbContext">Database context type.</typeparam>
internal class AuditConfiguration<TUserIdentifier, TDbContext> : IAuditConfiguration<TUserIdentifier, TDbContext>
    where TUserIdentifier : struct
    where TDbContext : DbContext
{
    /// <summary>
    /// Empty constructor.
    /// </summary>
    internal AuditConfiguration()
    {
    }

    /// <summary>
    /// Gets or sets a value indicating whether it should audit depending on if it has no changes in tracked properties.
    /// </summary>
    public bool DoNotAuditIfNoChangesInTrackedProperties { get; internal set; }

    /// <summary>
    /// Gets or sets Strategy.
    /// </summary>
    public AuditStrategy Strategy { get; internal set; }

    /// <summary>
    /// Gets or sets Entities.
    /// </summary>
    public IDictionary<Type, IEntityAuditConfiguration>? Entities { get; internal set; }

    /// <summary>
    /// Gets or sets UserIdentifierFactory.
    /// </summary>
    public Func<TUserIdentifier?>? UserIdentifierFactory { get; internal set; }

    /// <summary>
    /// Gets or sets SinkFactories.
    /// </summary>
    public IEnumerable<IAuditSinkFactory<TUserIdentifier, TDbContext>>? SinkFactories { get; set; }
}