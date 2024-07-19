using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers;

/// <summary>
/// Trigger context class.
/// </summary>
/// <typeparam name="TDbContext">Database context type of TriggerContext.</typeparam>
public class TriggerContext<TDbContext>
    where TDbContext : DbContext
{
    /// <summary>
    /// Constructor takes in an instance of EntityEntry, instance of DataBaseContext and an instance of EntityState.
    /// </summary>
    /// <param name="entityEntry">Instance of EntityEntry.</param>
    /// <param name="databaseContext">Instance of DatabaseContext.</param>
    /// <param name="initialEntityState">Instance of EntityState.</param>
    public TriggerContext(EntityEntry entityEntry, TDbContext databaseContext, EntityState initialEntityState)
    {
        EntityEntry = entityEntry;
        DbContext = databaseContext;
        InitialEntityState = initialEntityState;
    }

    /// <summary>
    /// Gets EntityEntry.
    /// </summary>
    public EntityEntry EntityEntry { get; }

    /// <summary>
    /// Gets DbContext.
    /// </summary>
    public TDbContext DbContext { get; }

    /// <summary>
    /// Gets InitialEntityState.
    /// </summary>
    public EntityState InitialEntityState { get; }
}
