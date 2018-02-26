using System;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public static class Trigger<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        public static event Action<TEntity, TriggerContext<TDbContext>> Inserting
        {
            add => TriggerRegistrar<TDbContext>.Register(TriggerType.Inserting, value);
            remove => TriggerRegistrar<TDbContext>.Revoke(TriggerType.Inserting, value);
        }

        public static event Action<TEntity, TriggerContext<TDbContext>> Inserted
        {
            add => TriggerRegistrar<TDbContext>.Register(TriggerType.Inserted, value);
            remove => TriggerRegistrar<TDbContext>.Revoke(TriggerType.Inserted, value);
        }

        public static event Action<TEntity, TriggerContext<TDbContext>> Updating
        {
            add => TriggerRegistrar<TDbContext>.Register(TriggerType.Updating, value);
            remove => TriggerRegistrar<TDbContext>.Revoke(TriggerType.Updating, value);
        }

        public static event Action<TEntity, TriggerContext<TDbContext>> Updated
        {
            add => TriggerRegistrar<TDbContext>.Register(TriggerType.Updated, value);
            remove => TriggerRegistrar<TDbContext>.Revoke(TriggerType.Updated, value);
        }

        public static event Action<TEntity, TriggerContext<TDbContext>> Deleting
        {
            add => TriggerRegistrar<TDbContext>.Register(TriggerType.Deleting, value);
            remove => TriggerRegistrar<TDbContext>.Revoke(TriggerType.Deleting, value);
        }

        public static event Action<TEntity, TriggerContext<TDbContext>> Deleted
        {
            add => TriggerRegistrar<TDbContext>.Register(TriggerType.Deleted, value);
            remove => TriggerRegistrar<TDbContext>.Revoke(TriggerType.Deleted, value);
        }
    }
}