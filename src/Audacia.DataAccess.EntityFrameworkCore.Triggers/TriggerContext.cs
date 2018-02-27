using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public class TriggerContext
    {
        public EntityEntry EntityEntry { get; set; }
        public DbContext DbContext { get; set; }
    }
}