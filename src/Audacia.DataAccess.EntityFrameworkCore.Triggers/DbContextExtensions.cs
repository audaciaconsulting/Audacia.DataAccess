using System;
using System.Threading;
using System.Threading.Tasks;
using Audacia.Core;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public static class DbContextExtensions
    {
        public static void AddSoftDeleteTrigger<TDbContext, TUserId>(this TDbContext dbContext,
            Func<TUserId> userIdFactory)
            where TDbContext : DbContext
        {
            Trigger<TDbContext, ISoftDeletable<TUserId>>.Deleting += (deletable, context) =>
            {
                context.EntityEntry.State = EntityState.Modified;
                deletable.Deleted = DateTimeOffsetProvider.Instance.Now;
                deletable.DeletedBy = userIdFactory();
            };
        }

        public static void AddCreateTrigger<TDbContext, TUserId>(this TDbContext dbContext,
            Func<TUserId> userIdFactory)
            where TDbContext : DbContext
        {
            Trigger<TDbContext, ICreatable<TUserId>>.Inserting += (creatable, context) =>
            {
                creatable.Created = DateTimeOffsetProvider.Instance.Now;
                creatable.CreatedBy = userIdFactory();
            };
        }

        public static void AddModifyTrigger<TDbContext, TUserId>(this TDbContext dbContext,
            Func<TUserId> userIdFactory)
            where TDbContext : DbContext
        {
            Trigger<TDbContext, IModifiable<TUserId>>.Updating += (modifiable, context) =>
            {
                modifiable.Modified = DateTimeOffsetProvider.Instance.Now;
                modifiable.ModifiedBy = userIdFactory();
            };
        }

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