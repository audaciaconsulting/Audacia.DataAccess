using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers;

/// <summary>
/// TriggerDispatcher class.
/// </summary>
/// <typeparam name="TDbContext">Type of database context of the <see cref="TriggerDispatcher{TDbContext}"/>.</typeparam>
internal class TriggerDispatcher<TDbContext>
    where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;
    private readonly TriggerRegistrar<TDbContext> _triggerRegistrar;

    /// <summary>
    /// Constructor initialise databaseContext and triggerRegistrar.
    /// </summary>
    /// <param name="databaseContext">Instance of databaseContext.</param>
    /// <param name="triggerRegistrar">Instance of triggerRegistrar.</param>
    public TriggerDispatcher(TDbContext databaseContext, TriggerRegistrar<TDbContext> triggerRegistrar)
    {
        _dbContext = databaseContext;
        _triggerRegistrar = triggerRegistrar;
    }

    /// <summary>
    /// DispachAsync method.
    /// </summary>
    /// <param name="triggerType">Instance of triggerType.</param>
    /// <param name="entityEntry">Instance of entityEntry.</param>
    /// <param name="initialEntityState">Instance of initialEntityState.</param>
    /// <param name="cancellationToken">CancellationToken.</param>
    /// <returns>Task.</returns>
    public Task DispatchAsync(TriggerType triggerType, EntityEntry entityEntry, EntityState initialEntityState,
        CancellationToken cancellationToken)
    {
        var entityType = entityEntry.Entity.GetType();

        var triggerContext = new TriggerContext<TDbContext>(entityEntry, _dbContext, initialEntityState);

        var delegates = _triggerRegistrar.Resolve(entityType, triggerType);

        var allTasks = new List<Task>();
        foreach (var task in delegates.Select(@delegate =>
            @delegate?.Invoke(entityEntry.Entity, triggerContext, cancellationToken))) 
        {
            if (task != null) 
            {
                allTasks.Add(task);
            }
        }

        return Task.WhenAll(allTasks);
    }

    /// <summary>
    /// DispatchBeforeAsync method.
    /// </summary>
    /// <param name="cancellationToken">CancellationToken.</param>
    /// <returns>Task.</returns>
    public async Task DispatchBeforeAsync(CancellationToken cancellationToken)
    {
        var func = _triggerRegistrar.ResolveBefore();

        if (func != null)
        {
            await func.Invoke(_dbContext, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// DispatchAfterAsync methos.
    /// </summary>
    /// <param name="cancellationToken">CancellationToken.</param>
    /// <returns>Task.</returns>
    public async Task DispatchAfterAsync(CancellationToken cancellationToken)
    {
        var func = _triggerRegistrar.ResolveAfter();

        if (func != null)
        {
            await func.Invoke(_dbContext, cancellationToken).ConfigureAwait(false);
        }
    }
}
