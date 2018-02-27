using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    public class AuditConfigurationRegistrar
    {
        public void AddConfiguration<TDbContext>(IAuditConfiguration<TDbContext> configuration)
            where TDbContext : DbContext
        {
            //TODO: Store Audits
        }
    }
}