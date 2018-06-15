using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    internal class TriggerInvoker<TDbContext>
        where TDbContext : DbContext
    {
        private readonly TriggerDispatcher<TDbContext> _dispatcher;
        private readonly ICollection<EntityEntry> _entityEntries;
        private readonly IDictionary<object, EntityState> _initalEntityStates;

        internal TriggerInvoker(TDbContext dbContext, TriggerRegistrar<TDbContext> registrar)
        {
            _dispatcher = new TriggerDispatcher<TDbContext>(dbContext, registrar);
            _entityEntries = dbContext.ChangeTracker.Entries().ToList();
            _initalEntityStates = new Dictionary<object, EntityState>();
        }
        
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

            await _dispatcher.DispatchBeforeAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            foreach (var entityEntry in _entityEntries)
            {
                cancellationToken.ThrowIfCancellationRequested();

                _initalEntityStates[entityEntry.Entity] = entityEntry.State;

                if(TryConvertEntityStateToBeforeTriggerType(entityEntry.State, out var triggerType))
                {
                    await _dispatcher.DispatchAsync(triggerType, entityEntry, entityEntry.State, cancellationToken);
                }
            }
        }
        
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
                    await _dispatcher.DispatchAsync(triggerType, entityEntry, initalEntityState, cancellationToken);
                }
            }

            cancellationToken.ThrowIfCancellationRequested();

            await _dispatcher.DispatchAfterAsync(cancellationToken);
        }
    }
}