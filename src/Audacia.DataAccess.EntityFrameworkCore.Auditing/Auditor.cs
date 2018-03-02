using System.Collections.Generic;
using System.Linq;
using Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;
using Audacia.DataAccess.EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public class Auditor<TDbContext>
        where TDbContext : DbContext
    {
        private readonly IEnumerable<AuditWorker<TDbContext>> _workers;

        public Auditor(IEnumerable<IAuditConfiguration<TDbContext>> configurations, IEnumerable<IAuditSink> sinks,
            TriggerRegistrar<TDbContext> triggerRegistrar)
        {
            _workers = configurations.Select(configuration =>
                new AuditWorker<TDbContext>(configuration, triggerRegistrar, sinks));
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