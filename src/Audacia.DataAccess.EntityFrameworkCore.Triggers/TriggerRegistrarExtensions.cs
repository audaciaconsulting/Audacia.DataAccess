using System;
using System.Threading.Tasks;
using Audacia.Core;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public static class TriggerRegistrarExtensions
    {
        public static void AddSoftDeleteTrigger<TUserId, TDbContext>(this TriggerRegistrar<TDbContext> registrar,
            Func<TUserId> userIdFactory)
            where TDbContext : DbContext where TUserId : struct
        {
            registrar.Type<ISoftDeletable<TUserId>>().DeletingAsync += (deletable, context, _) =>
            {
                context.EntityEntry.State = EntityState.Modified;
                deletable.Deleted = DateTimeOffsetProvider.Instance.Now;
                deletable.DeletedBy = userIdFactory();

                return Task.CompletedTask;
            };
        }

        public static void AddCreateTrigger<TUserId, TDbContext>(this TriggerRegistrar<TDbContext> registrar,
            Func<TUserId> userIdFactory) 
            where TDbContext : DbContext
        {
            registrar.Type<ICreatable<TUserId>>().InsertingAsync += (creatable, context, _) =>
            {
                creatable.Created = DateTimeOffsetProvider.Instance.Now;
                creatable.CreatedBy = userIdFactory();
                
                return Task.CompletedTask;
            };
        }

        public static void AddModifyTrigger<TUserId, TDbContext>(this TriggerRegistrar<TDbContext> registrar,
            Func<TUserId> userIdFactory) 
            where TDbContext : DbContext where TUserId : struct
        {
            registrar.Type<IModifiable<TUserId>>().UpdatingAsync += (modifiable, context, _) =>
            {
                modifiable.Modified = DateTimeOffsetProvider.Instance.Now;
                modifiable.ModifiedBy = userIdFactory();
                
                return Task.CompletedTask;
            };
        }
    }
}