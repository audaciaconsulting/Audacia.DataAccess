﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    internal class TriggerDispatcher<TDbContext>
        where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private readonly TriggerRegistrar<TDbContext> _triggerRegistrar;

        public TriggerDispatcher(TDbContext dbContext, TriggerRegistrar<TDbContext> triggerRegistrar)
        {
            _dbContext = dbContext;
            _triggerRegistrar = triggerRegistrar;
        }

        public Task DispatchAsync(TriggerType triggerType, EntityEntry entityEntry, EntityState initialEntityState,
            CancellationToken cancellationToken)
        {
            var entityType = entityEntry.Entity.GetType();

            var triggerContext = new TriggerContext<TDbContext>(entityEntry, _dbContext, initialEntityState);

            var delegates = _triggerRegistrar.Resolve(entityType, triggerType);

            return Task.WhenAll(delegates.Select(@delegate =>
                @delegate?.Invoke(entityEntry.Entity, triggerContext, cancellationToken)));
        }

        public async Task DispatchBeforeAsync(CancellationToken cancellationToken)
        {
            var func = _triggerRegistrar.ResolveBefore();

            if (func != null)
            {
                await func.Invoke(_dbContext, cancellationToken);
            }
        }

        public async Task DispatchAfterAsync(CancellationToken cancellationToken)
        {
            var func = _triggerRegistrar.ResolveAfter();

            if (func != null)
            {
                await func.Invoke(_dbContext, cancellationToken);
            }
        }
    }
}