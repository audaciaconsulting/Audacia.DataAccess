using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

/// <summary>
/// IAuditConfiguration interface.
/// </summary>
/// <typeparam name="TUserIdentifier">UserIdentifier struct stype.</typeparam>
/// <typeparam name="TDbContext">Database context type.</typeparam>
public interface IAuditConfiguration<TUserIdentifier, in TDbContext>
    where TUserIdentifier : struct
    where TDbContext : DbContext
{
    /// <summary>
    /// Gets a value indicating whether it should audit depending on if it has no changes in tracked properties.
    /// </summary>
    bool DoNotAuditIfNoChangesInTrackedProperties { get; }

    /// <summary>
    /// Gets Strategy.
    /// </summary>
    AuditStrategy Strategy { get; }

    /// <summary>
    /// Gets Entities.
    /// </summary>
    IDictionary<Type, IEntityAuditConfiguration>? Entities { get; }

    /// <summary>
    /// Gets UserIdentifierFactory.
    /// </summary>
    Func<TUserIdentifier?>? UserIdentifierFactory { get; }

    /// <summary>
    /// Gets SinkFactories.
    /// </summary>
    IEnumerable<IAuditSinkFactory<TUserIdentifier, TDbContext>>? SinkFactories { get; }
}