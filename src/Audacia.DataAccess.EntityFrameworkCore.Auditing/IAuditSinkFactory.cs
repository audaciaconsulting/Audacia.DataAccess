using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing;

public interface IAuditSinkFactory<TUserIdentifier, in TDbContext>
    where TDbContext : DbContext 
    where TUserIdentifier : struct
{
    IAuditSink<TUserIdentifier> Create(TDbContext context);
}