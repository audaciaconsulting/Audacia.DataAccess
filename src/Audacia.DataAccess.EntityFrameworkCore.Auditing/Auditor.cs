using System;
using System.Collections.Generic;
using System.Linq;
using Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;
using Audacia.DataAccess.EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public class Auditor<TUserIdentifier, TDbContext>
        where TDbContext : DbContext
        where TUserIdentifier : struct
    {
        private readonly IEnumerable<AuditWorker<TUserIdentifier, TDbContext>> _workers;

        public Auditor(IEnumerable<IAuditConfiguration<TDbContext>> configurations,
            IEnumerable<IAuditSinkFactory<TUserIdentifier, TDbContext>> sinkFactories,
            TriggerRegistrar<TDbContext> triggerRegistrar, Func<TUserIdentifier> userIdentifierFactory)
        {
            _workers = configurations.Select(configuration =>
                new AuditWorker<TUserIdentifier, TDbContext>(configuration, triggerRegistrar, sinkFactories, userIdentifierFactory));
        }

        public void Init()
        {
            foreach (var worker in _workers)
            {
                worker.Init();
            }
        }
    }
}