using System.Collections.Generic;
using Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public static class Audit<TDbContext>
        where TDbContext : DbContext
    {
        public static ICollection<AuditConfiguration<TDbContext>> Configurations { get; } =
            new List<AuditConfiguration<TDbContext>>();
    }
}