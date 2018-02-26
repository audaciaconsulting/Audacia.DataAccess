using System;
using Audacia.Core;
using Audacia.DataAccess.EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public static class DbContextExtensions
    {
        public static void AddSoftDeleteAuditing<TDbContext, TUserId>(this TDbContext dbContext,
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

        public static void AddCreateAuditing<TDbContext, TUserId>(this TDbContext dbContext,
            Func<TUserId> userIdFactory)
            where TDbContext : DbContext
        {
            Trigger<TDbContext, ICreatable<TUserId>>.Inserting += (creatable, context) =>
            {
                creatable.Created = DateTimeOffsetProvider.Instance.Now;
                creatable.CreatedBy = userIdFactory();
            };
        }

        public static void AddModifyAuditing<TDbContext, TUserId>(this TDbContext dbContext,
            Func<TUserId> userIdFactory)
            where TDbContext : DbContext
        {
            Trigger<TDbContext, IModifiable<TUserId>>.Updating += (modifiable, context) =>
            {
                modifiable.Modified = DateTimeOffsetProvider.Instance.Now;
                modifiable.ModifiedBy = userIdFactory();
            };
        }
    }
}