using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing;

/// <summary>
/// IAuditSinkFactory interface.
/// </summary>
/// <typeparam name="TUserIdentifier">UserIdentifier struct stype.</typeparam>
/// <typeparam name="TDbContext">Database context type.</typeparam>
public interface IAuditSinkFactory<TUserIdentifier, in TDbContext>
    where TUserIdentifier : struct
    where TDbContext : DbContext 
{
    /// <summary>
    /// Create method.
    /// </summary>
    /// <param name="context">Instance of database context.</param>
    /// <returns><see cref="IAuditSink{TUserIdentifier}"/>.</returns>
    IAuditSink<TUserIdentifier> Create(TDbContext context);
}