﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;
using Audacia.DataAccess.EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public static class DbContextExtensions
    {
        public static TDbContext EnableAuditing<TDbContext>(this TDbContext dbContext,
            AuditRegistrar<TDbContext> registrar)
            where TDbContext : DbContext
        {
            if (!dbContext.TriggersEnabled<TDbContext>())
            {
                throw new ApplicationException("You must enable triggers to use auditing.");
            }

            registrar.Enable();
            
            return dbContext;
        }
    }
}