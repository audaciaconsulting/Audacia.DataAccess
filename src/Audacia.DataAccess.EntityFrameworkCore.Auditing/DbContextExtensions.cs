using System;
using Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;
using Audacia.DataAccess.EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public static class DbContextExtensions
    {
        public static TDbContext EnableAuditing<TUserIdentifier, TDbContext>(this TDbContext dbContext,
            AuditRegistrar<TUserIdentifier, TDbContext> registrar)
            where TDbContext : DbContext
            where TUserIdentifier : struct
        {
            if (!dbContext.TriggersEnabled())
            {
                throw new ApplicationException("You must enable triggers to use auditing.");
            }

            registrar.Enable();
            
            return dbContext;
        }
    }
}