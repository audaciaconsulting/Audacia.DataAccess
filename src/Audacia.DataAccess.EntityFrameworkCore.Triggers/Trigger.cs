using System;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public class Trigger<TEntity>
        where TEntity : class
    {
        private readonly TriggerRegistrar _triggerRegistrar;

        internal Trigger(TriggerRegistrar triggerRegistrar)
        {
            _triggerRegistrar = triggerRegistrar;
        }

        public event Action<TEntity, TriggerContext> Inserting
        {
            add => _triggerRegistrar.Register(TriggerType.Inserting, value);
            remove => _triggerRegistrar.Revoke(TriggerType.Inserting, value);
        }

        public event Action<TEntity, TriggerContext> Inserted
        {
            add => _triggerRegistrar.Register(TriggerType.Inserted, value);
            remove => _triggerRegistrar.Revoke(TriggerType.Inserted, value);
        }

        public event Action<TEntity, TriggerContext> Updating
        {
            add => _triggerRegistrar.Register(TriggerType.Updating, value);
            remove => _triggerRegistrar.Revoke(TriggerType.Updating, value);
        }

        public event Action<TEntity, TriggerContext> Updated
        {
            add => _triggerRegistrar.Register(TriggerType.Updated, value);
            remove => _triggerRegistrar.Revoke(TriggerType.Updated, value);
        }

        public event Action<TEntity, TriggerContext> Deleting
        {
            add => _triggerRegistrar.Register(TriggerType.Deleting, value);
            remove => _triggerRegistrar.Revoke(TriggerType.Deleting, value);
        }

        public event Action<TEntity, TriggerContext> Deleted
        {
            add => _triggerRegistrar.Register(TriggerType.Deleted, value);
            remove => _triggerRegistrar.Revoke(TriggerType.Deleted, value);
        }
    }
}