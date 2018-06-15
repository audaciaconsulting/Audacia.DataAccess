using System;
using System.Collections.Generic;
using Audacia.DataAccess.EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace
    Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    public sealed class AuditRegistrar<TDbContext>
        where TDbContext : DbContext
    {
        private readonly TriggerRegistrar<TDbContext> _triggerRegistrar;

        private readonly ICollection<IAuditSinkFactory<TDbContext>> _sinkFactories = new List<IAuditSinkFactory<TDbContext>>();

        private readonly ICollection<IAuditConfiguration<TDbContext>> _configurations =
            new List<IAuditConfiguration<TDbContext>>();

        public AuditRegistrar(TriggerRegistrar<TDbContext> triggerRegistrar)
        {
            _triggerRegistrar = triggerRegistrar;
        }

        public AuditRegistrar<TDbContext> AddSinkFactory(IAuditSinkFactory<TDbContext> factory)
        {
            _sinkFactories.Add(factory);

            return this;
        }

        public AuditRegistrar<TDbContext> AddSinkFactory(Func<TDbContext, IAuditSink> factory)
        {
            _sinkFactories.Add(new DynamicAuditSinkFactory<TDbContext>(factory));

            return this;
        }

        public AuditRegistrar<TDbContext> AddConfiguration(IAuditConfiguration<TDbContext> configuration)
        {
            _configurations.Add(configuration);

            return this;
        }

        public void Enable()
        {
            var auditor = new Auditor<TDbContext>(_configurations, _sinkFactories, _triggerRegistrar);

            auditor.Init();
        }
    }
}