using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public interface IAuditSinkFactory<in TDbContext>
        where TDbContext : DbContext
    {
        IAuditSink Create(TDbContext context);
    }
}