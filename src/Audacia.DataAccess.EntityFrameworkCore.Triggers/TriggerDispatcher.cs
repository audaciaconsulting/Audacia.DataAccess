using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    internal class TriggerDispatcher
    {
        private readonly DbContext _dbContext;
        private readonly TriggerRegistrar _triggerRegistrar;

        public TriggerDispatcher(DbContext dbContext, TriggerRegistrar triggerRegistrar)
        {
            _dbContext = dbContext;
            _triggerRegistrar = triggerRegistrar;
        }

        internal void Dispatch(TriggerType triggerType, EntityEntry entityEntry)
        {
            var entityType = entityEntry.Entity.GetType();

            var triggerContext = new TriggerContext
            {
                DbContext = _dbContext,
                EntityEntry = entityEntry
            };

            var delegates = _triggerRegistrar.Resolve(entityType, triggerType);

            foreach (var @delegate in delegates)
            {
                @delegate.Invoke(entityEntry.Entity, triggerContext);
            }
        }
    }
}