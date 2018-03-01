using System.Collections.Generic;
using System.Linq;
using Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public class Auditor<TDbContext>
        where TDbContext : DbContext
    {
        private readonly IEnumerable<AuditConfigurationWorker<TDbContext>> _workers;

        public Auditor(IEnumerable<AuditConfiguration<TDbContext>> configurations)
        {
            _workers = configurations.Select(configuration => new AuditConfigurationWorker<TDbContext>(configuration));
        }

        public void Insterting(object obj, AuditContext context)
        {
            foreach (var worker in _workers)
            {
                worker.Insterting(obj, context);
            }
        }
        
        public void Inserted(object obj, AuditContext context)
        {
            foreach (var worker in _workers)
            {
                worker.Inserted(obj, context);
            }
        }

        public void Updating(object obj, AuditContext context)
        {
            foreach (var worker in _workers)
            {
                worker.Updating(obj, context);
            }
        }

        public void Updated(object obj, AuditContext context)
        {
            foreach (var worker in _workers)
            {
                worker.Updated(obj, context);
            }
        }

        public void Deleting(object obj, AuditContext context)
        {
            foreach (var worker in _workers)
            {
                worker.Deleting(obj, context);
            }
        }
    }
}