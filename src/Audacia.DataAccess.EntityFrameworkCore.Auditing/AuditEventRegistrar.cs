using System;
using System.Collections.Generic;
using Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;
using Audacia.DataAccess.EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public class AuditEventRegistrar<TDbContext>
        where TDbContext : DbContext
    {
        private readonly TriggerRegistrar _triggerRegistrar;
        private readonly AuditConfiguration<TDbContext> _auditConfiguration;

        private readonly IDictionary<TriggerType, Action<object, TriggerContext>> _triggers;

        public AuditEventRegistrar(TriggerRegistrar triggerRegistrar, Auditor<TDbContext> auditor,
            AuditConfiguration<TDbContext> auditConfiguration)
        {
            _triggerRegistrar = triggerRegistrar;
            _auditConfiguration = auditConfiguration;
            _triggers = new Dictionary<TriggerType, Action<object, TriggerContext>>
            {
                {TriggerType.Inserting, TransformToTrigger(auditor.Insterting)},
                {TriggerType.Inserted, TransformToTrigger(auditor.Inserted)},
                {TriggerType.Updating, TransformToTrigger(auditor.Updating)},
                {TriggerType.Updated, TransformToTrigger(auditor.Updated)},
                {TriggerType.Deleting, TransformToTrigger(auditor.Deleting)}
            };
        }

        public void Register()
        {
            foreach (var trigger in _triggers)
            {
                _triggerRegistrar.Register(trigger.Key, trigger.Value);
            }
        }

        public void Revoke()
        {
            foreach (var trigger in _triggers)
            {
                _triggerRegistrar.Revoke(trigger.Key, trigger.Value);
            }
        }
        
        public Action<object, TriggerContext> TransformToTrigger(Action<object, AuditContext> auditAction)
        {
            return (obj, triggerContext) => auditAction(obj, new AuditContext
            {
                Configuration = _auditConfiguration.Entities[obj.GetType()],
                TriggerContext = triggerContext
            });
        }
    }
}