using Audacia.DataAccess.EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

/// <summary>
/// AuditRegistrar class.
/// </summary>
/// <typeparam name="TUserIdentifier">UserIdentifier struct stype.</typeparam>
/// <typeparam name="TDbContext">Database context type.</typeparam>
public sealed class AuditRegistrar<TUserIdentifier, TDbContext>
    where TUserIdentifier : struct
    where TDbContext : DbContext
{
    private readonly TriggerRegistrar<TDbContext> _triggerRegistrar;

    /// <summary>
    /// Assigns value of triggerRegistrar.
    /// </summary>
    /// <param name="triggerRegistrar">Instance of <see cref="TriggerRegistrar{TDbContext}"/>.</param>
    public AuditRegistrar(TriggerRegistrar<TDbContext> triggerRegistrar)
    {
        _triggerRegistrar = triggerRegistrar;
    }

    /// <summary>
    /// Adds RegisterConfiguration.
    /// </summary>
    /// <param name="configuration">Instance of configuration <see cref="IAuditConfiguration{TUserIdentifier, TDbContext}"/>.</param>
    /// <returns><see cref="AuditRegistrar{TUserIdentifier, TDbContext}"/>.</returns>
    public AuditRegistrar<TUserIdentifier, TDbContext> RegisterConfiguration(IAuditConfiguration<TUserIdentifier, TDbContext> configuration)
    {
        new AuditConfigurationApplicant<TUserIdentifier, TDbContext>(_triggerRegistrar, configuration).Apply();

        return this;
    }
}