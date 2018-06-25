using System;
using System.Collections.Generic;
using Audacia.DataAccess.EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace
    Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    public sealed class AuditRegistrar<TUserIdentifier, TDbContext>
        where TDbContext : DbContext
        where TUserIdentifier : struct
    {
        private readonly TriggerRegistrar<TDbContext> _triggerRegistrar;
        private readonly Func<TUserIdentifier?> _userIdentifierFactory;

        private readonly ICollection<IAuditSinkFactory<TUserIdentifier, TDbContext>> _sinkFactories = new List<IAuditSinkFactory<TUserIdentifier, TDbContext>>();

        private readonly ICollection<IAuditConfiguration<TDbContext>> _configurations =
            new List<IAuditConfiguration<TDbContext>>();

        public AuditRegistrar(TriggerRegistrar<TDbContext> triggerRegistrar, Func<TUserIdentifier?> userIdentifierFactory)
        {
            _triggerRegistrar = triggerRegistrar;
            _userIdentifierFactory = userIdentifierFactory;
        }

        public AuditRegistrar<TUserIdentifier, TDbContext> AddSinkFactory(IAuditSinkFactory<TUserIdentifier, TDbContext> factory)
        {
            _sinkFactories.Add(factory);

            return this;
        }

        public AuditRegistrar<TUserIdentifier, TDbContext> AddSinkFactory(Func<TDbContext, IAuditSink<TUserIdentifier>> factory)
        {
            _sinkFactories.Add(new DynamicAuditSinkFactory<TUserIdentifier, TDbContext>(factory));

            return this;
        }

        public AuditRegistrar<TUserIdentifier, TDbContext> AddConfiguration(IAuditConfiguration<TDbContext> configuration)
        {
            _configurations.Add(configuration);

            return this;
        }

        public void Enable()
        {
            var auditor = new Auditor<TUserIdentifier, TDbContext>(_configurations, _sinkFactories, _triggerRegistrar, _userIdentifierFactory);

            auditor.Init();
        }
    }
}