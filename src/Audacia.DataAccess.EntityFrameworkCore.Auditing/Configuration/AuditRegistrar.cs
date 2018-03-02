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

        private readonly ICollection<IAuditSink> _sinks = new List<IAuditSink>();

        private readonly ICollection<IAuditConfiguration<TDbContext>> _configurations =
            new List<IAuditConfiguration<TDbContext>>();

        public AuditRegistrar(TriggerRegistrar<TDbContext> triggerRegistrar)
        {
            _triggerRegistrar = triggerRegistrar;
        }

        public AuditRegistrar<TDbContext> AddSink(IAuditSink sink)
        {
            _sinks.Add(sink);

            return this;
        }

        public AuditRegistrar<TDbContext> AddConfiguration(IAuditConfiguration<TDbContext> configuration)
        {
            _configurations.Add(configuration);

            return this;
        }

        public void Enable()
        {
            var auditor = new Auditor<TDbContext>(_configurations, _sinks, _triggerRegistrar);

            auditor.Init();
        }
    }
}