using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Audacia.Core;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public static class DbContextExtensions
    {
        private static readonly ConditionalWeakTable<DbContext, TriggerRegistrar> TriggerRegistrarConditionalWeakTable = new ConditionalWeakTable<DbContext, TriggerRegistrar>();

        public static TDbContext EnableTriggers<TDbContext>(this TDbContext dbContext, TriggerRegistrar registrar)
            where TDbContext : DbContext
        {
            TriggerRegistrarConditionalWeakTable.Add(dbContext, registrar);

            return dbContext;
        }

        public static bool TriggersEnabled(this DbContext dbContext)
        {
            return GetTriggerRegistrar(dbContext) != null;
        }

        private static TriggerRegistrar GetTriggerRegistrar(DbContext dbContext)
        {
            if (!TriggerRegistrarConditionalWeakTable.TryGetValue(dbContext, out var registrar))
            {
                throw new KeyNotFoundException(
                    $"You must call {nameof(EnableTriggers)} on the context before you can use triggers.");
            }

            return registrar;
        }

        public static Trigger<TEntity> Trigger<TEntity>(this DbContext dbContext)
            where TEntity : class
        {
            return new Trigger<TEntity>(GetTriggerRegistrar(dbContext));
        }

        public static void AddSoftDeleteTrigger<TUserId>(this DbContext dbContext,
            Func<TUserId> userIdFactory)
        {
            dbContext.Trigger<ISoftDeletable<TUserId>>().Deleting += (deletable, context) =>
            {
                context.EntityEntry.State = EntityState.Modified;
                deletable.Deleted = DateTimeOffsetProvider.Instance.Now;
                deletable.DeletedBy = userIdFactory();
            };
        }

        public static void AddCreateTrigger<TUserId>(this DbContext dbContext,
            Func<TUserId> userIdFactory)
        {
            dbContext.Trigger<ICreatable<TUserId>>().Inserting += (creatable, context) =>
            {
                creatable.Created = DateTimeOffsetProvider.Instance.Now;
                creatable.CreatedBy = userIdFactory();
            };
        }

        public static void AddModifyTrigger<TUserId>(this DbContext dbContext,
            Func<TUserId> userIdFactory)
        {
            dbContext.Trigger<IModifiable<TUserId>>().Updating += (modifiable, context) =>
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
            var invoker = new TriggerInvoker(dbContext, GetTriggerRegistrar(dbContext));

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
            var invoker = new TriggerInvoker(dbContext, GetTriggerRegistrar(dbContext));

            invoker.Before();
            var result = dbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            invoker.After();

            return result;
        }
    }
}