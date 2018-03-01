using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace
    Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    public sealed class AuditRegistrar<TDbContext>
        where TDbContext : DbContext
    {
        internal IList<IAuditConfiguration<TDbContext>> Configurations = new List<IAuditConfiguration<TDbContext>>();
        internal IList<IAuditSink> Sinks = new List<IAuditSink>();

        public void AddConfiguration(IAuditConfiguration<TDbContext> configuration)
        {
            Configurations.Add(configuration);
        }

        public void AddSink(IAuditSink sink)
        {
            Sinks.Add(sink);
        }
    }
}