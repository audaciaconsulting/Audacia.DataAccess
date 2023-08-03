using System;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing;

internal class DynamicAuditSinkFactory<TUserIdentifier, TDbContext> : IAuditSinkFactory<TUserIdentifier, TDbContext>
    where TDbContext : DbContext 
    where TUserIdentifier : struct
{
    private readonly Func<TDbContext, IAuditSink<TUserIdentifier>> _factory;

    public DynamicAuditSinkFactory(Func<TDbContext, IAuditSink<TUserIdentifier>> factory)
    {
        _factory = factory;
    }

    public IAuditSink<TUserIdentifier> Create(TDbContext context) => _factory.Invoke(context);
}