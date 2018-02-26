using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public static class DbContextExtensions
    {
        public static int SaveChangesWithTriggers<TDbContext>(this TDbContext dbContext) 
            where TDbContext : DbContext =>
            dbContext.SaveChangesWithTriggers(true);

        public static int SaveChangesWithTriggers<TDbContext>(this TDbContext dbContext, bool acceptAllChangesOnSuccess)
            where TDbContext : DbContext
        {
            var invoker = new TriggerInvoker<TDbContext>(dbContext);

            invoker.Before();
            var result = dbContext.SaveChanges(acceptAllChangesOnSuccess);
            invoker.After();

            return result;
        }

        public static Task<int> SaveChangesWithTriggersAsync<TDbContext>(this TDbContext dbContext,
            CancellationToken cancellationToken = default)
            where TDbContext : DbContext => 
            dbContext.SaveChangesWithTriggersAsync(true, cancellationToken);

        public static Task<int> SaveChangesWithTriggersAsync<TDbContext>(this TDbContext dbContext, bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
            where TDbContext : DbContext
        {
            var invoker = new TriggerInvoker<TDbContext>(dbContext);

            invoker.Before();
            var result = dbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            invoker.After();

            return result;
        }
    }
}