using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Audacia.CodeAnalysis.Analyzers.Helpers.MethodLength;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers;

/// <summary>
/// TriggerInvoker class.
/// </summary>
/// <typeparam name="TDbContext">Type of database context of the <see cref="TriggerInvoker{TDbContext}"/>.</typeparam>
internal class TriggerInvoker<TDbContext>
    where TDbContext : DbContext
{
    private readonly TriggerDispatcher<TDbContext> _dispatcher;
    private readonly ICollection<EntityEntry> _entityEntries;
    private readonly IDictionary<object, EntityState> _initalEntityStates;

    /// <summary>
    /// Constructor assigns databaseContext and registrar values.
    /// </summary>
    /// <param name="databaseContext">Instance of databaseContext.</param>
    /// <param name="registrar">Instance of registrar.</param>
    internal TriggerInvoker(TDbContext databaseContext, TriggerRegistrar<TDbContext> registrar)
    {
        _dispatcher = new TriggerDispatcher<TDbContext>(databaseContext, registrar);
        _entityEntries = databaseContext.ChangeTracker.Entries().ToList();
        _initalEntityStates = new ConcurrentDictionary<object, EntityState>();
    }

    /// <summary>
    /// BeforeAsync method.
    /// </summary>
    /// <param name="cancellationToken">Cancellationtoken.</param>
    /// <returns>Task.</returns>
    [MaxMethodLength(16)]
    public async Task BeforeAsync(CancellationToken cancellationToken)
    {
        bool TryConvertEntityStateToBeforeTriggerType(EntityState entityState, out TriggerType triggerType)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (entityState)
            {
                case EntityState.Added:
                    triggerType = TriggerType.Inserting;
                    return true;
                case EntityState.Modified:
                    triggerType = TriggerType.Updating;
                    return true;
                case EntityState.Deleted:
                    triggerType = TriggerType.Deleting;
                    return true;
                default:
                    triggerType = default;
                    return false;
            }
        }

        await _dispatcher.DispatchBeforeAsync(cancellationToken).ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        foreach (var entityEntry in _entityEntries)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _initalEntityStates[entityEntry.Entity] = entityEntry.State;

            if (TryConvertEntityStateToBeforeTriggerType(entityEntry.State, out var triggerType))
            {
                await _dispatcher.DispatchAsync(triggerType, entityEntry, entityEntry.State, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// AfterAsync method.
    /// </summary>
    /// <param name="cancellationToken">Cancellationtoken.</param>
    /// <returns>Task.</returns>
    [MaxMethodLength(16)]
    public async Task AfterAsync(CancellationToken cancellationToken)
    {
        bool TryConvertEntityStateToAfterTriggerType(EntityState entityState, out TriggerType triggerType)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (entityState)
            {
                case EntityState.Added:
                    triggerType = TriggerType.Inserted;
                    return true;
                case EntityState.Modified:
                    triggerType = TriggerType.Updated;
                    return true;
                case EntityState.Deleted:
                    triggerType = TriggerType.Deleted;
                    return true;
                default:
                    triggerType = default;
                    return false;
            }
        }

        foreach (var entityEntry in _entityEntries)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var initalEntityState = _initalEntityStates[entityEntry.Entity];

            if (TryConvertEntityStateToAfterTriggerType(initalEntityState, out var triggerType))
            {
                await _dispatcher.DispatchAsync(triggerType, entityEntry, initalEntityState, cancellationToken).ConfigureAwait(false);
            }
        }

        cancellationToken.ThrowIfCancellationRequested();

        await _dispatcher.DispatchAfterAsync(cancellationToken).ConfigureAwait(false);
    }
}
