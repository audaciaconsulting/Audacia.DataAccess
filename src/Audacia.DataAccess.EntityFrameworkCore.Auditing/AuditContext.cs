using Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;
using Audacia.DataAccess.EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing;

/// <summary>
/// AuditContext class.
/// </summary>
/// <typeparam name="TDbContext">Database context type.</typeparam>
public class AuditContext<TDbContext>
    where TDbContext : DbContext
{
    /// <summary>
    /// Constructor assigns configuration and triggerContext values.
    /// </summary>
    /// <param name="configuration">Instance of configuration.</param>
    /// <param name="triggerContext">Instance of triggerContext.</param>
    public AuditContext(IEntityAuditConfiguration configuration, TriggerContext<TDbContext> triggerContext)
    {
        Configuration = configuration;
        TriggerContext = triggerContext;
    }

    /// <summary>
    /// Gets configuration.
    /// </summary>
    public IEntityAuditConfiguration Configuration { get; }

    /// <summary>
    /// Gets triggerContext.
    /// </summary>
    public TriggerContext<TDbContext> TriggerContext { get; }
}