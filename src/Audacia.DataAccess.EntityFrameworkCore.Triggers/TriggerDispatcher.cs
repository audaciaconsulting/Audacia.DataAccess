using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    internal class TriggerDispatcher<TDbContext>
        where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public TriggerDispatcher(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        internal void Dispatch(TriggerType triggerType, EntityEntry entityEntry)
        {
            var entityType = entityEntry.Entity.GetType();

            var triggerContext = new TriggerContext<TDbContext>
            {
                DbContext = _dbContext,
                EntityEntry = entityEntry
            };

            var delegates = TriggerRegistrar<TDbContext>.Resolve(entityType, triggerType);

            foreach (var @delegate in delegates)
            {
                @delegate.Invoke(entityEntry.Entity, triggerContext);
            }
        }
    }
}