using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public class TriggerTypeRegistrar<TDbContext, T>
        where T : class
        where TDbContext : DbContext
    {
        private readonly TriggerRegistrar<TDbContext> _triggerRegistrar;

        internal TriggerTypeRegistrar(TriggerRegistrar<TDbContext> triggerRegistrar)
        {
            _triggerRegistrar = triggerRegistrar;
        }
        
        public event Func<T, TriggerContext<TDbContext>, CancellationToken, Task> InsertingAsync
        {
            add => _triggerRegistrar.Register(TriggerType.Inserting, value);
            remove => _triggerRegistrar.Revoke(TriggerType.Inserting, value);
        }

        public event Func<T, TriggerContext<TDbContext>, CancellationToken, Task> InsertedAsync
        {
            add => _triggerRegistrar.Register(TriggerType.Inserted, value);
            remove => _triggerRegistrar.Revoke(TriggerType.Inserted, value);
        }

        public event Func<T, TriggerContext<TDbContext>, CancellationToken, Task> UpdatingAsync
        {
            add => _triggerRegistrar.Register(TriggerType.Updating, value);
            remove => _triggerRegistrar.Revoke(TriggerType.Updating, value);
        }

        public event Func<T, TriggerContext<TDbContext>, CancellationToken, Task> UpdatedAsync
        {
            add => _triggerRegistrar.Register(TriggerType.Updated, value);
            remove => _triggerRegistrar.Revoke(TriggerType.Updated, value);
        }

        public event Func<T, TriggerContext<TDbContext>, CancellationToken, Task> DeletingAsync
        {
            add => _triggerRegistrar.Register(TriggerType.Deleting, value);
            remove => _triggerRegistrar.Revoke(TriggerType.Deleting, value);
        }

        public event Func<T, TriggerContext<TDbContext>, CancellationToken, Task> DeletedAsync
        {
            add => _triggerRegistrar.Register(TriggerType.Deleted, value);
            remove => _triggerRegistrar.Revoke(TriggerType.Deleted, value);
        }
    }
}