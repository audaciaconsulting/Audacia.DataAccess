using System;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing;

/// <summary>
/// DynamicAuditSinkFactory class. 
/// </summary>
/// <typeparam name="TUserIdentifier">UserIdentifier struct stype.</typeparam>
/// <typeparam name="TDbContext">Database context type.</typeparam>
internal class DynamicAuditSinkFactory<TUserIdentifier, TDbContext> : IAuditSinkFactory<TUserIdentifier, TDbContext>
    where TUserIdentifier : struct
    where TDbContext : DbContext 
{
    private readonly Func<TDbContext, IAuditSink<TUserIdentifier>> _factory;

    /// <summary>
    /// Assigns factory value <see cref="Func{TDbContext,IAuditSink}" />.
    /// </summary>
    /// <param name="factory">Instance of <see cref="Func{TDbContext,IAuditSink}" />..</param>
    public DynamicAuditSinkFactory(Func<TDbContext, IAuditSink<TUserIdentifier>> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Create a new <see cref="IAuditSink{TUserIdentifier}"/>.
    /// </summary>
    /// <param name="context">Instance of database context.</param>
    /// <returns><see cref="IAuditSink{TUserIdentifier}"/>.</returns>
    public IAuditSink<TUserIdentifier> Create(TDbContext context) => _factory.Invoke(context);
}