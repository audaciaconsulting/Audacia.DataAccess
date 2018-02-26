using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public class TriggerContext<TDbContext>
        where TDbContext : DbContext
    {
        public EntityEntry EntityEntry { get; set; }
        public TDbContext DbContext { get; set; }
    }
}