using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public static class DbContextExtensions
    {
        private static readonly ConditionalWeakTable<DbContext, object> TriggerRegistrarConditionalWeakTable =
            new ConditionalWeakTable<DbContext, object>();

        public static TDbContext EnableTriggers<TDbContext>(this TDbContext dbContext,
            TriggerRegistrar<TDbContext> registrar)
            where TDbContext : DbContext
        {
            TriggerRegistrarConditionalWeakTable.Add(dbContext, registrar);

            return dbContext;
        }

        public static bool TriggersEnabled<TDbContext>(this TDbContext dbContext)
            where TDbContext : DbContext
        {
            return GetTriggerRegistrar(dbContext) != null;
        }

        private static TriggerRegistrar<TDbContext> GetTriggerRegistrar<TDbContext>(TDbContext dbContext)
            where TDbContext : DbContext
        {
            if (!TriggerRegistrarConditionalWeakTable.TryGetValue(dbContext, out var registrar))
            {
                throw new KeyNotFoundException(
                    $"You must call {nameof(EnableTriggers)} on the context before you can use triggers.");
            }

            return registrar as TriggerRegistrar<TDbContext>;
        }

        public static Task<int> SaveChangesWithTriggersAsync<TDbContext>(this TDbContext dbContext,
            Func<bool, CancellationToken, Task<int>> baseSaveChangesAsync,
            CancellationToken cancellationToken = default)
            where TDbContext : DbContext =>
            dbContext.SaveChangesWithTriggersAsync(baseSaveChangesAsync, true, cancellationToken);

        public static async Task<int> SaveChangesWithTriggersAsync<TDbContext>(this TDbContext dbContext,
            Func<bool, CancellationToken, Task<int>> baseSaveChangesAsync,
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
            where TDbContext : DbContext
        {
            var registrar = GetTriggerRegistrar(dbContext);

            var invoker = new TriggerInvoker<TDbContext>(dbContext, registrar);

            int result;

            cancellationToken.ThrowIfCancellationRequested();

            using (var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                await invoker.BeforeAsync(cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                result = await baseSaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                await invoker.AfterAsync(cancellationToken);

                //Last chance
                cancellationToken.ThrowIfCancellationRequested();

                transaction.Commit();
            }

            return result;
        }
    }
}