using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;
using Audacia.DataAccess.EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public static class DbContextExtensions
    {
        private static readonly ConditionalWeakTable<DbContext, AuditConfigurationRegistrar>
            AuditConfigurationRegistrarConditionalWeakTable =
                new ConditionalWeakTable<DbContext, AuditConfigurationRegistrar>();

        public static TDbContext EnableAuditing<TDbContext>(this TDbContext dbContext,
            AuditConfigurationRegistrar registrar)
            where TDbContext : DbContext
        {
            if (!dbContext.TriggersEnabled())
            {
                throw new ApplicationException("You must enable triggers to use auditing.");
            }

            AuditConfigurationRegistrarConditionalWeakTable.Add(dbContext, registrar);

            return dbContext;
        }

        public static bool AuditingEnabled(this DbContext dbContext)
        {
            return GetAuditConfigurationRegistrar(dbContext) != null;
        }

        private static AuditConfigurationRegistrar GetAuditConfigurationRegistrar(DbContext dbContext)
        {
            if (!AuditConfigurationRegistrarConditionalWeakTable.TryGetValue(dbContext, out var registrar))
            {
                throw new KeyNotFoundException(
                    $"You must call {nameof(EnableAuditing)} on the context before you can use audit.");
            }

            return registrar;
        }

        public static TDbContext Audit<TDbContext>(this TDbContext dbContext,
            IAuditConfiguration<TDbContext> configuration)
            where TDbContext : DbContext
        {
            var registrar = GetAuditConfigurationRegistrar(dbContext);
            registrar.AddConfiguration(configuration);

            return dbContext;
        }
    }
}