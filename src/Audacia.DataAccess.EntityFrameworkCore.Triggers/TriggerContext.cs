using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public class TriggerContext<TDbContext>
        where TDbContext : DbContext
    {
        public TriggerContext(EntityEntry entityEntry, TDbContext dbContext, EntityState initialEntityState)
        {
            EntityEntry = entityEntry;
            DbContext = dbContext;
            InitialEntityState = initialEntityState;
        }

        public EntityEntry EntityEntry { get; }

        public TDbContext DbContext { get; }

        public EntityState InitialEntityState { get; }
    }
}