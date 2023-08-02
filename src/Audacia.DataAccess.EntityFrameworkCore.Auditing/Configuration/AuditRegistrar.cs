using System;
using System.Collections.Generic;
using Audacia.DataAccess.EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

public sealed class AuditRegistrar<TUserIdentifier, TDbContext>
    where TDbContext : DbContext
    where TUserIdentifier : struct
{
    private readonly TriggerRegistrar<TDbContext> _triggerRegistrar;

    public AuditRegistrar(TriggerRegistrar<TDbContext> triggerRegistrar)
    {
        _triggerRegistrar = triggerRegistrar;
    }

    public AuditRegistrar<TUserIdentifier, TDbContext> RegisterConfiguration(IAuditConfiguration<TUserIdentifier, TDbContext> configuration)
    {
        new AuditConfigurationApplicant<TUserIdentifier, TDbContext>(_triggerRegistrar, configuration).Apply();

        return this;
    }
}