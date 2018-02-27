using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    internal class TriggerInvoker
    {
        private readonly TriggerDispatcher _dispatcher;
        private readonly ICollection<EntityEntry> _entityEntries;

        internal TriggerInvoker(DbContext dbContext, TriggerRegistrar registrar)
        {
            _dispatcher = new TriggerDispatcher(dbContext, registrar);
            _entityEntries = dbContext.ChangeTracker.Entries().ToList();
        }
        
        public void Before()
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

            foreach (var entityEntry in _entityEntries)
            {
                if(TryConvertEntityStateToBeforeTriggerType(entityEntry.State, out var triggerType))
                {
                    _dispatcher.Dispatch(triggerType, entityEntry);
                }
            }
        }
        
        public void After()
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
                if (TryConvertEntityStateToAfterTriggerType(entityEntry.State, out var triggerType))
                {
                    _dispatcher.Dispatch(triggerType, entityEntry);
                }
            }
        }
    }
}