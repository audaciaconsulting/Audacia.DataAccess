using System;
using System.Threading.Tasks;
using Audacia.Core;
using Audacia.DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public static class TriggerRegistrarExtensions
    {
        public static void AddSoftDeleteTrigger<TUserIdentifier, TDbContext>(this TriggerRegistrar<TDbContext> registrar,
            Func<TUserIdentifier?> userIdentifierFactory)
            where TDbContext : DbContext
            where TUserIdentifier : struct
        {
            registrar.Type<ISoftDeletable<TUserIdentifier>>().DeletingAsync += (deletable, context, _) =>
            {
                context.EntityEntry.State = EntityState.Modified;
                deletable.Deleted = DateTimeOffsetProvider.Instance.Now;
                deletable.DeletedBy = userIdentifierFactory();

                return Task.CompletedTask;
            };
        }

        public static void AddCreateTrigger<TUserIdentifier, TDbContext>(this TriggerRegistrar<TDbContext> registrar,
            Func<TUserIdentifier?> userIdentifierFactory) 
            where TDbContext : DbContext
            where TUserIdentifier : struct
        {
            registrar.Type<ICreatable<TUserIdentifier>>().InsertingAsync += (creatable, context, _) =>
            {
                creatable.Created = DateTimeOffsetProvider.Instance.Now;
                creatable.CreatedBy = userIdentifierFactory();
                
                return Task.CompletedTask;
            };
        }

        public static void AddModifyTrigger<TUserIdentifier, TDbContext>(this TriggerRegistrar<TDbContext> registrar,
            Func<TUserIdentifier?> userIdentifierFactory) 
            where TDbContext : DbContext
            where TUserIdentifier : struct
        {
            registrar.Type<IModifiable<TUserIdentifier>>().UpdatingAsync += (modifiable, context, _) =>
            {
                modifiable.Modified = DateTimeOffsetProvider.Instance.Now;
                modifiable.ModifiedBy = userIdentifierFactory();
                
                return Task.CompletedTask;
            };
        }
    }
}